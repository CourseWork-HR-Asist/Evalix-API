using Newtonsoft.Json;

namespace Application.Services.LLM;

public class LLMSummaryResponse
{
    [JsonProperty("summary_en")]
    public string SummaryEn { get; set; } = string.Empty;
}

public class LLMExtractResponse : LLMSummaryResponse
{
    public IEnumerable<string> Skills { get; set; } = [];
}

public class LLMMatchReponse : LLMSummaryResponse
{
    [JsonProperty("match_percentage")]
    public string MatchScore { get; set; } = string.Empty;
}