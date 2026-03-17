using Practice_Quiz_Generator.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class GitHubGeminiAdapter:IGeminiService
    {
        private readonly GitHubModelsService _gitHubService;

        public GitHubGeminiAdapter(GitHubModelsService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        public async Task<string> GetLLMResponseAsync(string prompt)
        {
            return await _gitHubService.GenerateContentAsync(prompt);
        }
    }
}
