using System.Net.Http.Json;
using System.Text.Json;
using Application.Services.Interfaces;
using Newtonsoft.Json;

namespace Application.Services.LLM;

public class LLMService(LLMSetting lLMSetting, HttpClient httpClient) : ILLMService
{
    public static IEnumerable<string> Endpoints => ["extract_skills", "extract_skills_from_pdf"];

    public async Task<LLMResponse> ExtractSkillsAsync(string query)
    {
        var payload = new { query };
        var response = await httpClient.PostAsJsonAsync(Endpoints.ElementAt(0), payload);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LLMResponse>(content)!;
    }

    public async Task<LLMResponse> ExtractSkillsFromPdfAsync(Stream pdfStream, string fileName)
    {
        ArgumentNullException.ThrowIfNull(pdfStream);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));

        using var content = new MultipartFormDataContent
        {
            { new StreamContent(pdfStream), "file", fileName }
        };

        var response = await httpClient.PostAsync(Endpoints.ElementAt(1), content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LLMResponse>(responseBody)!;
    }
}
