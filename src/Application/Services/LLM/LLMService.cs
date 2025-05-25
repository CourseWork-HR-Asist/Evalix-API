using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Application.Services.Interfaces;
using Newtonsoft.Json;

namespace Application.Services.LLM;

enum Endpoints
{
    ExtractSkills = 0,
    ExtractSkillsFromPdf,
    MatchResume,
    MatchResumeFromPdf,
    GetSummary
}

public static partial class StringExtension
{
    [GeneratedRegex("([a-z0-9])([A-Z])", RegexOptions.Compiled)]
    private static partial Regex UnderscoreRule();

    public static string ToUnderscore(this string input)
        => UnderscoreRule().Replace(input, "$1_$2").ToLower().Trim('_');
}

public class LLMService(LLMSetting lLMSetting, HttpClient httpClient) : ILLMService
{
    private static readonly Dictionary<Endpoints, string> EndpointsDict = Enum.GetValues<Endpoints>()
        .ToDictionary(endpoint => endpoint, endpoint => endpoint.ToString().ToUnderscore());

    public async Task<LLMExtractResponse> ExtractSkillsAsync(string query)
    {
        var payload = new { query };
        var response = await httpClient.PostAsJsonAsync(EndpointsDict[Endpoints.ExtractSkills], payload);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LLMExtractResponse>(content)!;
    }

    public async Task<LLMExtractResponse> ExtractSkillsFromPdfAsync(Stream pdfStream, string fileName)
    {
        ArgumentNullException.ThrowIfNull(pdfStream);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));

        using var content = new MultipartFormDataContent
        {
            { new StreamContent(pdfStream), "file", fileName }
        };

        var response = await httpClient.PostAsync(EndpointsDict[Endpoints.ExtractSkillsFromPdf], content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LLMExtractResponse>(responseBody)!;
    }

    public async Task<LLMMatchReponse> MatchRequirementsAsync(string resume, string requirement)
    {
        if (string.IsNullOrWhiteSpace(resume))
            throw new ArgumentNullException(nameof(resume));

        if (string.IsNullOrWhiteSpace(requirement))
            throw new ArgumentNullException(nameof(requirement));

        var payload = new { resume, requirement };
        var response = await httpClient.PostAsJsonAsync(EndpointsDict[Endpoints.MatchResume], payload);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LLMMatchReponse>(content)!;
    }

    public async Task<LLMMatchReponse> MatchRequirementsFromPdfAsync(Stream pdfStream, string fileName, string requirement)
    {
        ArgumentNullException.ThrowIfNull(pdfStream);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));

        if (string.IsNullOrWhiteSpace(requirement))
            throw new ArgumentNullException(nameof(requirement));

        using var content = new MultipartFormDataContent
        {
            { new StreamContent(pdfStream), "file", fileName },
            { new StringContent(requirement), "requirement" }
        };
        var response = await httpClient.PostAsync(EndpointsDict[Endpoints.MatchResumeFromPdf], content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<LLMMatchReponse>(responseBody)!;
    }

    public async Task<LLMSummaryResponse> SummarizeResumeAsync(Stream pdfStream, string fileName)
    {
        ArgumentNullException.ThrowIfNull(pdfStream);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));

        using var content = new MultipartFormDataContent
        {
            { new StreamContent(pdfStream), "pdf_file", fileName }
        };

        var response = await httpClient.PostAsync(EndpointsDict[Endpoints.GetSummary], content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LLMSummaryResponse>(responseBody)!;
    }
}
