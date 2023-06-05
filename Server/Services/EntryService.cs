using Server.Data;
using Shared.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Server.Services;

public interface IEntryService
{
    // Task<IEnumerable<Entry>> GetEntriesAsync();
    // Task<Entry?> GetEntryAsync(int id);
    // Task DeleteEntryAsync(int id);
    Task<Entry> UpdateEntryAsync(Entry entry);
    Task<Entry> AddEntryAsync(Entry entry);
    Task<Entry?> GetEntryByNumberPlateAsync(string numberPlate);
    decimal CalculateAmount(DateTime from, DateTime to);
}

public class EntryService : IEntryService
{
    private readonly CpmDbContext _context;
    private readonly IRules _ruleService = Rules.Instance;

    public EntryService(CpmDbContext context)
    {
        _context = context;
    }

    // public async Task<IEnumerable<Entry>> GetEntriesAsync()
    // {
    //     return await _context.Entries.ToListAsync();
    // }

    // public async Task<Entry?> GetEntryAsync(int id)
    // {
    //     var entry = await _context.Entries.FindAsync(id);
    //     if (entry == null) return null;
    //     return entry;
    // }

    // public async Task DeleteEntryAsync(int id)
    // {
    //     var entry = await _context.Entries.FindAsync(id);
    //     if (entry == null) return;

    //     entry.Deleted = DateTime.Now;
    //     _context.Entry(entry).State = EntityState.Modified;
    //     await _context.SaveChangesAsync();
    // }

    public async Task<Entry> UpdateEntryAsync(Entry entry)
    {
        entry.Updated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        entry.To = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        _context.Entry(entry).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<Entry> AddEntryAsync(Entry entry)
    {
        entry.Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        entry.From = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        entry.Status = StatusEnum.Parked;
        _context.Entries.Add(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public Task<Entry?> GetEntryByNumberPlateAsync(string numberPlate)
    {
        return _context.Entries.OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync(e => e.NumberPlate == numberPlate && e.Status == StatusEnum.Parked);
    }


    public decimal CalculateAmount(DateTime from, DateTime to)
    {
        var curFrom = from;
        var rules = _ruleService.GetRules();
        var totalAmount = 0m;
        Rule? runningRule = null;
        var ruleRunMins = 0;

        // run for every day
        while (curFrom < to)
        {
            var curDay = curFrom.DayOfWeek;
            var rulesForDay = rules[curDay];

            if (runningRule != null)
            {
                totalAmount = getAmount(totalAmount, runningRule, ruleRunMins);
                ruleRunMins = 0;
            }

            foreach (var kvp in rulesForDay)
            {
                var rule = kvp.Value;
                // store a rule into running rule, it can be used at the end with ruleRunMins to calculate the amount
                if (runningRule == null)
                    runningRule = rule;

                // check if the rule is valid
                if (rule.UseAgainBefore != null && rule.UseAgainBefore <= (curFrom - from).TotalMinutes)
                    continue;

                // check if the curFrom is within the rule range
                // curFrom should be greater than or equal to rule From
                while (curFrom.TimeOfDay < rule.Day.From)
                    curFrom = curFrom.AddMinutes(1);

                // curFrom should be less than rule To
                if (curFrom.TimeOfDay >= rule.Day.To)
                    continue;

                // get out of invalid range
                var shouldBreak = false;
                while (rule.ValidEnd == null && (curFrom - from).TotalMinutes <= rule.ValidStart)
                {
                    curFrom = curFrom.AddMinutes(1);
                    if (curFrom > to)
                        shouldBreak = true;
                    if (curFrom.DayOfWeek != curDay)
                        shouldBreak = true;

                    if (shouldBreak)
                        break;
                }
                if (shouldBreak)
                    break;

                // for unlimited end time rules
                if (rule.ValidEnd == null && (curFrom - from).TotalMinutes >= rule.ValidStart)
                {
                    if (runningRule.Name != rule.Name)
                    {
                        totalAmount = getAmount(totalAmount, runningRule, ruleRunMins);
                        ruleRunMins = 0;
                        runningRule = rule;
                    }

                    ruleRunMins += 1;
                    curFrom = curFrom.AddMinutes(rule.Minutes);
                    ruleRunMins += rule.Minutes;
                }
                else
                {
                    curFrom = curFrom.AddMinutes(1);

                    // change the rule if its not same
                    if (runningRule.Name == rule.Name)
                        ruleRunMins += 1;
                    else
                    {
                        totalAmount = getAmount(totalAmount, runningRule, ruleRunMins);
                        ruleRunMins = 1;
                        runningRule = rule;
                    }
                }

                // post check                
                if (curFrom > to || curFrom.DayOfWeek != curDay)
                {
                    ruleRunMins -= 1;
                    break;
                }
            }
        }

        if (runningRule != null && ruleRunMins > 0)
        {
            totalAmount = getAmount(totalAmount, runningRule, ruleRunMins);
        }

        return totalAmount;
    }

    private static decimal getAmount(decimal totalAmount, Rule runningRule, int ruleRunMins)
    {
        if (runningRule.CanProRate) totalAmount += runningRule.Amount * (ruleRunMins / runningRule.Minutes);
        else
        {
            var multiply = (int)Math.Ceiling((decimal)ruleRunMins / runningRule.Minutes);
            totalAmount += multiply * runningRule.Amount;
        }

        return totalAmount;
    }
}

