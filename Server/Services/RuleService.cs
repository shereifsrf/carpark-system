
using Shared.Models;

namespace Server.Services;

public interface IRules
{
    void GenerateRules();
    Dictionary<DayOfWeek, Dictionary<int, Rule>> GetRules();
}

public class Rules : IRules
{
    private Rules()
    {
        GenerateRules();
    }
    private static Rules? instance = null;
    public static Rules Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Rules();
            }
            return instance;
        }
    }

    private readonly Dictionary<DayOfWeek, Dictionary<int, Rule>> rules = new();
    // public Dictionary<DayOfWeek, List<Rule>> GetRules() => rules;

    public Dictionary<DayOfWeek, Dictionary<int, Rule>> GetRules() => rules;
    public void GenerateRules()
    {
        // add day of week as key of dict
        // TODO: order every value as list by ValidStart
        var ruleList = getRuleList();
        foreach (var rule in ruleList)
        {
            foreach (var day in rule.Days)
            {
                if (!rules.ContainsKey(day.DayOfWeek))
                {
                    rules.Add(day.DayOfWeek, new Dictionary<int, Rule>());
                }

                var ruleObj = new Rule(rule.Name, rule.Minutes, rule.Amount, rule.CanProRate, rule.UseAgainBefore, rule.ValidStart, rule.ValidEnd, day);

                // if validend is null, that mean, just make sure only can use if total minutes is more than validstart
                if (rule.ValidEnd == null) rules[day.DayOfWeek].Add(rule.ValidStart, ruleObj);
                else for (int i = rule.ValidStart + 1; i <= rule.ValidEnd; i++) rules[day.DayOfWeek].Add(i, ruleObj);
            }
        }
    }

    private IList<RuleBatch> getRuleList()
    {
        return new List<RuleBatch> {
            new RuleBatch("First 30m - Weekday", DateConstant.Minute30, 0.2m, true, DateConstant.Day, 0, DateConstant.Minute30, DateConstant.Weekdays, DateConstant.WorkingHours),
            new RuleBatch("Second 30m - Weekday", DateConstant.Minute30, 0.5m, true,DateConstant.Day, DateConstant.Minute30, DateConstant.Hour, DateConstant.Weekdays, DateConstant.WorkingHours),
            new RuleBatch("After 1h - Weekday", DateConstant.Minute30, 1.6m, true, DateConstant.Day, DateConstant.Hour, DateConstant.DayMinus1h, DateConstant.Weekdays, DateConstant.WorkingHours),
            new RuleBatch("After 24h - Weekday", DateConstant.Day,      50m, false, null, DateConstant.Day, null, DateConstant.Weekdays, null),
            new RuleBatch("After 24h - Weekend", DateConstant.Day,      25m, false, null, DateConstant.Day, null, DateConstant.Weekends, null)
        };
    }
}