using System.Net.Http.Json;
using CPM.Shared.Utils;
using CPM.Shared.ClientServices;
using CPM.Shared.DTO;

namespace CPM.Client.Services;

public interface IParkService: IClientParkService
{
    Task<Tuple<decimal?, ErrorStatusDTO?>> GetTotalEarning(DateTime from, DateTime to);
    Task<Tuple<int?, ErrorStatusDTO?>> GetTotalParked(DateTime from, DateTime to);
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

    public async Task<Tuple<decimal?, ErrorStatusDTO?>> GetTotalEarning(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(Api.TotalAmount + $"?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var totalAmount = await response.Content.ReadFromJsonAsync<decimal>();
            return new Tuple<decimal?, ErrorStatusDTO?>(totalAmount, null);
        }
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return new Tuple<decimal?, ErrorStatusDTO?>(0, errorStatus);
        }
    }

    public async Task<Tuple<int?, ErrorStatusDTO?>> GetTotalParked(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(Api.TotalParked + $"?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var totalParked = await response.Content.ReadFromJsonAsync<int>();
            return new Tuple<int?, ErrorStatusDTO?>(totalParked, null);
        }
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return new Tuple<int?, ErrorStatusDTO?>(0, errorStatus);
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