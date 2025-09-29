namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GetLLMResponseAsync(string prompt);
    }
}
