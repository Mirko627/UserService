using System.Net.Http.Json;
using UserService.ClientHttp.Interfaces;
using UserService.Shared.dtos;

namespace UserService.ClientHttp.Clients
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(IHttpClientFactory httpClientFactory)
        {
            // Assicurati che nel Program.cs di chi usa questo client 
            // sia configurato un client chiamato "UserApiClient"
            _httpClient = httpClientFactory.CreateClient("UserApiClient");
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
            var response = await _httpClient.PostAsJsonAsync("api/user", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, UpdateUserDto user)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/{id}", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/user/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return null;

            // Leggiamo l'oggetto anonimo { token: "..." }
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }
    }

    // Helper interno per mappare la risposta del login
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}