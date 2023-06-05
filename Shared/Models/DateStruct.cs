public struct DateConstant
{
    public readonly static List<DayOfWeek> Weekdays = new() {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };
    public readonly static List<DayOfWeek> Weekends = new() {
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };
    public readonly static Tuple<TimeSpan, TimeSpan> WorkingHours = new(new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0));
    public readonly static List<Tuple<TimeSpan, TimeSpan>> NonWorkingHours = new(){
            //  17:00:00 to 09:00:00, cut into two parts
            new Tuple<TimeSpan, TimeSpan>(new TimeSpan(17, 0, 0), new TimeSpan(24, 0, 0)),
            new Tuple<TimeSpan, TimeSpan>(new TimeSpan(0, 0, 0), new TimeSpan(9, 0, 0))
        };

    public readonly static int Minute = 1;
    public readonly static int Minute30 = 30 * Minute;
    public readonly static int Hour = 60 * Minute;
    public readonly static int Day = 24 * Hour;
    public readonly static int DayMinus1h = Day - Hour;
}