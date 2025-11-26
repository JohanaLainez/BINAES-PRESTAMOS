using System.Net.Http.Json;

namespace Binaes.Web.Services;

public class BinaesApiClient
{
    private readonly HttpClient _http;

    public BinaesApiClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("BinaesApi");
    }

    public async Task<List<T>> GetListAsync<T>(string path)
        => await _http.GetFromJsonAsync<List<T>>(path) ?? new();

    public async Task<T?> GetAsync<T>(string path)
        => await _http.GetFromJsonAsync<T>(path);

    public async Task<bool> PostAsync<T>(string path, T body)
        => (await _http.PostAsJsonAsync(path, body)).IsSuccessStatusCode;

    public async Task<bool> PutAsync<T>(string path, T body)
        => (await _http.PutAsJsonAsync(path, body)).IsSuccessStatusCode;

    public async Task<bool> DeleteAsync(string path)
        => (await _http.DeleteAsync(path)).IsSuccessStatusCode;
}
