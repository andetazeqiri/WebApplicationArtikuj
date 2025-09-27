using Newtonsoft.Json;
using System.Text;
using ArtikujManager.Web.Models;

namespace ArtikujManager.Web.Services
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string endpoint, string? token = null) where T : class;
        Task<T?> PostAsync<T>(string endpoint, object data, string? token = null) where T : class;
        Task<T?> PutAsync<T>(string endpoint, object data, string? token = null) where T : class;
        Task<bool> DeleteAsync(string endpoint, string? token = null);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5276";
        }

        public async Task<T?> GetAsync<T>(string endpoint, string? token = null) where T : class
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}{endpoint}");
                
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }

                throw new HttpRequestException($"API call failed: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calling API: {ex.Message}", ex);
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data, string? token = null) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{endpoint}")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }

                throw new HttpRequestException($"API call failed: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calling API: {ex.Message}", ex);
            }
        }

        public async Task<T?> PutAsync<T>(string endpoint, object data, string? token = null) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}{endpoint}")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }

                throw new HttpRequestException($"API call failed: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calling API: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(string endpoint, string? token = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}{endpoint}");

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calling API: {ex.Message}", ex);
            }
        }
    }
}