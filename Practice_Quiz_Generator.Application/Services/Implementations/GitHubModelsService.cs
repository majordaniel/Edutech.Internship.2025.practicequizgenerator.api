using DocumentFormat.OpenXml.EMMA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class GitHubModelsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _model;
        private readonly ILogger<GitHubModelsService> _logger;

        public GitHubModelsService(HttpClient httpClient, IConfiguration configuration, ILogger<GitHubModelsService> logger)
        {
            _httpClient = httpClient;
            _token = configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHub token not found");
            _model = configuration["GitHubModels:Model"] ?? "gpt-4o-mini";
            _logger = logger;
        }

        public async Task<string> GenerateContentAsync(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "system",
                            content = "You are a helpful AI grading assistant that returns responses in JSON format.",
                        },
                        new
                        {
                            role = "user",
                            content = prompt,
                        }
                    },
                    model = _model,
                    temperature = 0.7,
                    max_tokens = 1000
                };

                var request = new HttpRequestMessage(
                HttpMethod.Post,
                    "https://models.inference.ai.azure.com/chat/completions"
            );

                request.Headers.Add("Authorization", $"Bearer {_token}");

                request.Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                _logger.LogInformation("Calling GitHub Models API with model: {Model}", _model);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("GitHub Models API Error: {StatusCode} - {Content}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"API Error: {response.StatusCode}");
                }

                using var jsonDoc = JsonDocument.Parse(responseContent);

                var content = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return content ?? "No response";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling GitHub Models API");
                throw;
            }
        }
    }
}
