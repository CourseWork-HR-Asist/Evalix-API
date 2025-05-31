using Application.Services.Interfaces;
using Application.Services.LLM;

namespace Application.Services.LLMSettings;

public class LLMSettingsService(LLMSetting settings) : ILLMSettingsService
{
    private readonly LLMSetting _current = settings;

    public LLMSetting Settings => _current;

    public void Update(LLMSetting updated)
    {
        _current.Url = updated.Url;
    }
}