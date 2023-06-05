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