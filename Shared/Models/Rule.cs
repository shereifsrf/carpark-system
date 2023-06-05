namespace Shared.Models;


public class Rule : BasicRule
{
    public Day Day { get; set; }
    public Rule(string name, int minutes, decimal amount, bool canProRate, int? useAgainBefore, int validStart, int? validEnd, Day day)
    : base(name, minutes, amount, canProRate, useAgainBefore, validStart, validEnd)
    {
        Day = day;
    }
}

public class BasicRule
{
    public string Name { get; set; }
    public int Minutes { get; set; }
    public decimal Amount { get; set; }
    public bool CanProRate { get; set; }
    public int? UseAgainBefore { get; set; }
    public int ValidStart { get; set; }
    public int? ValidEnd { get; set; }

    public BasicRule(
        string name,
        int minutes,
        decimal amount,
        bool canProRate,
        int? useAgainBefore,
        int validStart,
        int? validEnd)
    {
        Name = name;
        Minutes = minutes;
        Amount = amount;
        CanProRate = canProRate;
        UseAgainBefore = useAgainBefore;
        ValidStart = validStart;
        ValidEnd = validEnd;
    }
}

public class RuleBatch : BasicRule
{
    public List<Day> Days { get; set; }
    public RuleBatch(string name, int minutes, decimal amount, bool canProRate, int? useAgainBefore, int validStart, int? validEnd,
        List<DayOfWeek> days,
        Tuple<TimeSpan, TimeSpan>? workingHours
    ) : base(name, minutes, amount, canProRate, useAgainBefore, validStart, validEnd)
    {
        Days = new List<Day>();
        foreach (var day in days)
        {
            Days.Add(new Day
            {
                DayOfWeek = day,
                From = workingHours?.Item1,
                To = workingHours?.Item2
            });
        }
    }
}

public class Day
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan? From { get; set; }
    public TimeSpan? To { get; set; }
}