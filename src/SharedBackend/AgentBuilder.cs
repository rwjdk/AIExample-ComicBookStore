using Microsoft.Agents.AI;
using System.ClientModel;
using Azure.AI.OpenAI;
using OpenAI;

namespace SharedBackend;

public static class AgentBuilder
{
    public static AzureOpenAIClient GetClient(BackendConfiguration backendConfiguration)
    {
        return new AzureOpenAIClient(
            new Uri(backendConfiguration.AzureOpenAIEndpoint),
            new ApiKeyCredential(backendConfiguration.AzureOpenAIKey));
    }

    public static AIAgent GetComicBookGuy(BackendConfiguration backendConfiguration)
    {
        AIAgent comicBookGuyAgent = GetClient(backendConfiguration)
            .GetChatClient(backendConfiguration.ComicBookGuyModel)
            .CreateAIAgent(instructions: "You are comic-book-guy from the Simpsons. Do not use Markdown in the answers")
            .AsBuilder()
            .UseOpenTelemetry("ComicBookGuySource", telemetryAgent => telemetryAgent.EnableSensitiveData = true)
            .Build();

        return comicBookGuyAgent;
    }

    public static AIAgent GetAssistant(BackendConfiguration backendConfiguration)
    {
        AIAgent assistantAgent = GetClient(backendConfiguration)
            .GetChatClient(backendConfiguration.AssistantModel)
            .CreateAIAgent(instructions: "You are comic-book-guy from the Simpsons sane assistant when he become a bit too much. Do not use Markdown in the answers")
            .AsBuilder()
            .UseOpenTelemetry("AssistantSource", telemetryAgent => telemetryAgent.EnableSensitiveData = true)
            .Build();

        return assistantAgent;
    }
}