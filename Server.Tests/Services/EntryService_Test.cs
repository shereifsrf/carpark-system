using Xunit;
using Server.Services;

namespace Server.Tests.Services;

// test for CalculateAmount

public class EntryService_Test
{
    // Mon - Fri working hours (9AM to 5PM)
    // first 30minutes => 0.2$ per 30 minutes
    // second 30minutes => 0.5$ per 30 minutes
    // after 1 hour => 1.6$ per 30 minutes
    // after 24 hours => 50$ per day

    // Sat - Sun and not working hours
    // first 30minutes => free
    // second 30minutes => free
    // after 1 hour => free
    // after 24 hours => 25$ per day

    // for eg: enter 2021-01-01 13:10:00, exit 2021-01-01 14:25:00
    // total time = 1 hour 15 minutes
    // first 30 minutes => 0.2$, ie 13:10:00 to 13:40:00
    // second 30 minutes => 0.5$, ie 13:40:00 to 14:10:00
    // after 1 hour => 15minutes * 1.6$ / 30minutes = 0.8$, ie 14:10:00 to 14:25:00
    // total amount = 0.2$ + 0.5$ + 0.8$ = 1.5$

    // for eg: enter 2021-01-01 16:30:00, exit 2021-01-01 20:20:00
    // total time = 3 hours 50 minutes
    // first 30 minutes => 0.2$, ie 16:30:00 to 17:00:00
    // second 30 minutes => falls on non-working hours, ie 17:00:00 to 18:20:00
    // total amount = 0.2$

    // for eg: enter 2021-01-04 14:00:00, exit 2021-01-05 16:00:00
    // total time = 26 hours
    // first 30 minutes => 0.2$, ie 14:00:00 to 14:30:00
    // second 30 minutes => 0.5$, ie 14:30:00 to 15:00:00
    // after 1 hour until 17:00:00 => 2 hours * 1.6 / 30minutes => 4 * 1.6$ => 6.4$, ie 15:00:00 to 17:00:00
    // 17:00:00 to next day 09:00:00 => free
    // total hours until now => 30minutes + 30minutes + 2hours + 16hours => 19hours
    // next day 
    // first 30 minutes => 0.2$, ie 09:00:00 to 09:30:00, ie 19hours + 30minutes
    // second second 30 minutes => 0.5$, ie 09:30:00 to 10:00:00, ie 19hours + 30minutes + 30minutes => 20hours
    // after 1 hour until 16:00:00 => 6hours * 1.6 / 30minutes => 12 * 1.6$ => 19.2$, ie 10:00:00 to 16:00:00, ie 20hours + 6hours => 26hours
    // after 24 hours => 50, ie 14:00:00 to 16:00:00
    // total amount = 0.2$ + 0.5$ + 6.4$ + 0.2$ + 0.5$ + 19.2$ + 50$ = 77$
    public static IEnumerable<object[]> CalculateAmountData =>
        new List<object[]>
        {            
            // 0.2
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 12, 30, 0), 0.2 },
            // 0.2+0.5 = 0.7
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 13, 0, 0), 0.7 },
            // 0.2+0.5+1*1.6 = 2.3
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 13, 30, 0), 2.3 },
            // 0.2+0.5+(2*1.6) = 3.9
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 14, 0, 0), 3.9 },
            // 0.2+0.5+ (4*1.6) = 7.1
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 15, 0, 0), 7.1 },
            // 0.2+0.5+ (8*1.6) = 13.5
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 17, 0, 0), 13.5 },
            // 0.2+0.5+ (8*1.6) = 13.5 || 7h30m
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 2, 17, 30, 0), 13.5 },
            // 0.2+0.5+ (8*1.6) = 13.5 || 12h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 0, 0, 0), 13.5 },
            // 0.2+0.5+ (8*1.6) = 13.5 || 20h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 8, 0, 0), 13.5 },
            // 0.2+0.5+ (8*1.6) = 13.5 || 21h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 9, 0, 0), 13.5 },
            // 0.2+0.5+8*1.6 = 13.5 || 22h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 10, 0, 0), 13.5 },
            // 0.2+0.5+8*1.6 || 13.5 || 23h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 11, 0, 0), 13.5 },
            // 0.2+0.5+8*1.6 || 13.5 || 24h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 12, 0, 0), 13.5 },
            // 0.2+0.5+8*1.6 || +25 => 38.5 || 25h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 12, 1, 0), 38.5 },
            // 0.2+0.5+8*1.6 || +25 => 38.5 || 26h
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 3, 13, 0, 0), 38.5 },
            // 4 days, weekday, weekend, weekend, weekday
            // day 1: 0.2+0.5+8*1.6 = 13.5
            // day 2: 25
            // day 3: 25
            // day 4: 50
            // total: 113.5
            new object[] { new DateTime(2023, 6, 2, 12, 0, 0), new DateTime(2023, 6, 5, 13, 0, 0), 113.5 },

            // free time
            new object[] { new DateTime(2023, 6, 1, 23, 0, 0), new DateTime(2023, 6, 2, 0, 0, 0), 0 },
            new object[] { new DateTime(2023, 6, 1, 18, 0, 0), new DateTime(2023, 6, 1, 18, 30, 0), 0 },

            //  how if we have 2 working days, should check if can use first 30m again
            // 0.2+0.5+ (8*1.6) = 13.5
            new object[] { new DateTime(2023, 6, 1, 12, 0, 0), new DateTime(2023, 6, 2, 9, 0, 0), 13.5 },
            // 0.2+0.5+ (8*1.6) || +0.2 = 13.7 || 9h30m
            new object[] { new DateTime(2023, 6, 1, 12, 0, 0), new DateTime(2023, 6, 2, 9, 30, 0), 13.7 },
            // 0.2+0.5+ (8*1.6) || +0.2+0.5 = 14.2 
            new object[] { new DateTime(2023, 6, 1, 12, 0, 0), new DateTime(2023, 6, 2, 10, 00, 0), 14.2 },
            // 0.2+0.5+8*1.6 || 0.2+0.5+4*1.6 => 20.6 || 26h
            new object[] { new DateTime(2023, 6, 1, 12, 0, 0), new DateTime(2023, 6, 2, 12, 0, 0), 20.6 },
            // 0.2+0.5+8*1.6 || 0.2+0.5+4*1.6 + 50 => 70.6 || 26h50m
            new object[] { new DateTime(2023, 6, 1, 12, 0, 0), new DateTime(2023, 6, 2, 12, 1, 0), 70.6 },
        };

    [Theory, MemberData(nameof(CalculateAmountData))]
    public void CalculateAmount_ShouldReturnAmount(DateTime from, DateTime to, decimal expected)
    {
        // Arrange
        var entryService = new EntryService(null);

        // Act
        var actual = entryService.CalculateAmount(from, to);

        // Assert
        Assert.Equal(expected, actual);
    }
}