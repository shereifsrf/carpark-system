using Shared.Enums;

namespace CPM.Shared.DTO;

public class EntryDTO
{
    public string NumberPlate { get; set; } = null!;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public StatusEnum Status { get; set; }
    public decimal Amount { get; set; }
}

public class EntryParkRequestDTO
{
    public string NumberPlate { get; set; } = null!;
}

public class EntryUnParkDTO
{
    public string NumberPlate { get; set; } = null!;
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;

    public string ParkingTime { get; set; } = null!;
    public string Amount { get; set; }
    private string dateFormat = "dd/MM/yy 'at' hh:mm tt";

    public EntryUnParkDTO(EntryDTO entry)
    {
        NumberPlate = "Car Number: " + entry.NumberPlate;
        From = "Date & Time of Entry: " + entry.From.ToString(dateFormat);
        To = "Date & Time of Exit: " + entry.To.ToString(dateFormat);
        ParkingTime = "Parking Time: " + parkingTime(entry.From, entry.To);
        Amount = "Total Amount: MR " + entry.Amount;
    }

    // parking time in 1.30 where 1 is hour and 30 is minutes
    private string parkingTime(DateTime From, DateTime To)
    {
        var time = To - From;
        // total hour and minute, it can be many days but show in hour and minute
        var hour = time.Hours + (time.Days * 24);
        return $"{hour}.{time.Minutes}";
    }
}