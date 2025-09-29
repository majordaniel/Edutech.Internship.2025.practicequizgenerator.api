using Microsoft.Extensions.Configuration;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config.GetSection("LLMSettings")["ApiKey"];

            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Gemini API key is missing. Please check LLMSettings:ApiKey in configuration.");
        }


        public async Task<string> GetLLMResponseAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                    "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent"
            );

            request.Headers.Add("X-goog-api-key", _apiKey);

            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
                using var responseContent = await response.Content.ReadAsStreamAsync();
                using var jsonDoc = await JsonDocument.ParseAsync(responseContent);

                var text = jsonDoc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "No response from LLM";
        }
    }
}
