using Newtonsoft.Json;

namespace Application.Services.LLM;

public class LLMResponse
{
    public IEnumerable<string> Skills { get; set; } = [];
    [JsonProperty("summary_en")]
    public string SummaryEn { get; set; } = string.Empty;
}
