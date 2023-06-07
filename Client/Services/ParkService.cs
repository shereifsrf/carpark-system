using System.Net.Http.Json;
using CPM.Shared.Utils;
using CPM.Shared.ClientServices;
using CPM.Shared.DTO;
using CPM.Shared.Models;

namespace CPM.Client.Services;

public interface IParkService : IClientParkService
{
    Task<ApiResponse<decimal>> GetTotalEarning(DateTime from, DateTime to);
    Task<ApiResponse<int>> GetTotalParked(DateTime from, DateTime to);
}

public class ParkService : ClientParkService, IParkService
{
    private readonly HttpClient _httpClient;

    public ParkService(HttpClient httpClient) : base(httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<decimal>> GetTotalEarning(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(Api.TotalAmount + $"?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var totalAmount = await response.Content.ReadFromJsonAsync<decimal>();
            return new () { Data = totalAmount };
        }
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return new() { ErrorStatus = errorStatus };
        }
    }

    public async Task<ApiResponse<int>> GetTotalParked(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(Api.TotalParked + $"?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var totalParked = await response.Content.ReadFromJsonAsync<int>();
            return new() { Data = totalParked };
        }
        else
        {
            var errorStatus = await response.Content.ReadFromJsonAsync<ErrorStatusDTO>();
            return new() { ErrorStatus = errorStatus };
        }
    }
}