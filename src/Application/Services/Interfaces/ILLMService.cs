using Application.Services.LLM;

namespace Application.Services.Interfaces;

public interface ILLMService
{
    Task<LLMExtractResponse> ExtractSkillsAsync(string query);
    Task<LLMExtractResponse> ExtractSkillsFromPdfAsync(Stream pdfStream, string fileName);
    Task<LLMMatchReponse> MatchRequirementsAsync(string resume, string requirement);
    Task<LLMMatchReponse> MatchRequirementsFromPdfAsync(Stream pdfStream, string fileName, string requirement);
    Task<LLMSummaryResponse> SummarizeResumeAsync(Stream pdfStream, string fileName);
}
