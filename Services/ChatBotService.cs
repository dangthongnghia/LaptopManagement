using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LaptopManagement.Services
{
    public class ChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;
        private readonly string _apiKey;

        public ChatBotService(string apiEndpoint, string apiKey)
        {
            _httpClient = new HttpClient();
            _apiEndpoint = apiEndpoint;
            _apiKey = apiKey;
            
            // Configure HTTP client with authentication header
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> SendMessageAsync(string message, string userId)
        {
            try
            {
                var requestData = new 
                {
                    message = message,
                    userId = userId
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<BotResponse>(jsonResponse);
                    return responseData?.Reply ?? "No response from bot";
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Error communicating with bot: {ex.Message}";
            }
        }
    }

    public class BotResponse
    {
        public string Reply { get; set; }
    }
}
