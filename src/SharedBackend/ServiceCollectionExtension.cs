using Microsoft.Extensions.Configuration;

namespace SharedBackend;

public static class ServiceCollectionExtension
{
    public static BackendConfiguration GetBackendConfiguration(this ConfigurationManager configurationManager)
    {
        string azureOpenAIEndpoint = configurationManager["CBS_AZURE_OPEN_AI_ENDPOINT"] ?? throw new ApplicationException("azureOpenAIEndpoint env. variable is missing");
        string azureOpenAIKey = configurationManager["CBS_AZURE_OPEN_AI_KEY"] ?? throw new ApplicationException("azureOpenAIKey env. variable is missing");
        string comicBookGuyModel = configurationManager["CBS_COMIC_BOOK_GUY_AGENT_MODEL"] ?? throw new ApplicationException("comic-book-guy-agent-model env. variable is missing");
        string assistantModel = configurationManager["CBS_ASSISTANT_AGENT_MODEL"] ?? throw new ApplicationException("assistant-agent-model env. variable is missing");

        return new BackendConfiguration(azureOpenAIEndpoint, azureOpenAIKey, comicBookGuyModel, assistantModel);
    }
}