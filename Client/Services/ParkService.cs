using System.Net.Http.Json;
using CPM.Client.Utils;
using CPM.Shared.DTO;

namespace CPM.Client.Services;

public interface IParkService
{
    Task<string?> Park(string numberPlate);
    Task<Tuple<EntryDTO?, ErrorStatusDTO?>> UnPark(string numberPlate);
}

public class ParkService : IParkService
{
    private readonly HttpClient _httpClient;

    public ParkService(HttpClient httpClient)
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

    public async Task<Tuple<EntryDTO?, ErrorStatusDTO?>> UnPark(string numberPlate)
    {
        var response = await _httpClient.PostAsJsonAsync(Api.UnPark, new EntryParkRequestDTO { NumberPlate = numberPlate });
        if (response.IsSuccessStatusCode)
        {
            var entryDTO = await response.Content.ReadFromJsonAsync<EntryDTO>();
            return new Tuple<EntryDTO?, ErrorStatusDTO?>(entryDTO, null);
        }
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return new Tuple<EntryDTO?, ErrorStatusDTO?>(null, errorStatus);
        }
    }
}