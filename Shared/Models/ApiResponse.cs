using CPM.Shared.DTO;

namespace CPM.Shared.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public ErrorStatusDTO? ErrorStatus { get; set; }
}