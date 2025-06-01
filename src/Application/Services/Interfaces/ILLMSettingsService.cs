using Application.Services.LLM;

namespace Application.Services.Interfaces;

public interface ILLMSettingsService
{
    LLMSetting Settings { get; }
    void Update(LLMSetting updated);
}
