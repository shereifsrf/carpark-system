namespace CPM.Shared.DTO;

public class ErrorStatusDTO
{
    public static ErrorStatusDTO NumberPlateAlreadyExists = new()
    {
        Message = "Number plate already exists"
    };
    public static ErrorStatusDTO NumberPlateNotParked = new()
    {
        Message = "Vehicle with number plate is not parked"
    };

    // parking slot is full
    public static ErrorStatusDTO FullyParked = new()
    {
        Message = "Fully parked"
    };
    // NumberPlateNotValid
    public static ErrorStatusDTO NumberPlateNotValid = new()
    {
        Message = "Number plate is not valid"
    };
    public string Message { get; set; } = null!;
}