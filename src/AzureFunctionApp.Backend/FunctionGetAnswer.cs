using System;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorWasm.FrontEnd.Pages;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using SharedBackend;
using SharedModels;

namespace AzureFunctionApp.Backend;

public class FunctionGetAnswer(BackendConfiguration backendConfiguration)
{
    [Function("FunctionGetAnswer")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        AnswerRequest answerRequest = (await JsonSerializer.DeserializeAsync<AnswerRequest>(req.Body))!;

        AIAgent agent;
        switch (answerRequest.Persona)
        {
            case ChatPersona.ComicBookGuy:
                agent = AgentBuilder.GetComicBookGuy(backendConfiguration);
                break;
            case ChatPersona.Assistant:
                agent = AgentBuilder.GetAssistant(backendConfiguration);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AgentRunResponse response = await agent.RunAsync(answerRequest.Question);
        return new OkObjectResult(response.Text);
    }
}