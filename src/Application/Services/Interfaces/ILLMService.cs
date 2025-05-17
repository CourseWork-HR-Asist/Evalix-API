using Application.Services.LLM;

namespace Application.Services.Interfaces;

public interface ILLMService
{
    Task<LLMResponse> ExtractSkillsAsync(string query);
    Task<LLMResponse> ExtractSkillsFromPdfAsync(Stream pdfStream, string fileName);
}
