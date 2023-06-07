using System.Net.Http.Json;
using CPM.Shared.Utils;
using CPM.Shared.DTO;
using CPM.Shared.Models;

namespace CPM.Shared.ClientServices;

public interface IClientParkService
{
    Task<string?> Park(string numberPlate);
    Task<ApiResponse<EntryUnParkDTO>> UnPark(string numberPlate);
}

public class ClientParkService : IClientParkService
{
    private readonly HttpClient _httpClient;

    public ClientParkService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> Park(string numberPlate)
    {
        var response = await _httpClient.PostAsJsonAsync(Api.Park, new EntryParkRequestDTO { NumberPlate = numberPlate });
        if (response.IsSuccessStatusCode)
            return null;
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return errorStatus?.Message;
        }
    }

    public async Task<ApiResponse<EntryUnParkDTO>> UnPark(string numberPlate)
    {
        var response = await _httpClient.PostAsJsonAsync(Api.UnPark, new EntryParkRequestDTO { NumberPlate = numberPlate });
        if (response.IsSuccessStatusCode)
        {
            var entryDTO = await response.Content.ReadFromJsonAsync<EntryDTO>();
            return new() { Data = new EntryUnParkDTO(entryDTO!) };
        }
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return new() { ErrorStatus = errorStatus };
        }
    }
}