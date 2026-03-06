using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UserService.ClientHttp.Interfaces;
using UserService.Shared.dtos;

namespace UserService.ClientHttp.Clients
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/user") ?? [];
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/user/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task AddAsync(CreateUserDto user)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/user");
            request.Content = JsonContent.Create(user);

            AddAuthorizationHeader(request);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, UpdateUserDto user)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"api/user/{id}");
            request.Content = JsonContent.Create(user);

            AddAuthorizationHeader(request);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        public async Task ChangePasswordAsync(int id, ChangePasswordDto passwordDto)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/user/change-password/{id}");
            request.Content = JsonContent.Create(passwordDto);

            AddAuthorizationHeader(request);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteAsync(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"api/user/{id}");

            AddAuthorizationHeader(request);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/user/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }
        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            string? authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}