using Blazored.LocalStorage;
using JetBrains.Annotations;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.AGUI;
using Microsoft.Extensions.AI;

namespace BlazorWasm.FrontEnd.Pages;

[UsedImplicitly]
public partial class Home(ILocalStorageService localStorageService, HttpClient httpClient)
{
    private string? _question;
    private string? _answer;
    private ChatPersona _selectedPersona = ChatPersona.ComicBookGuy;
    private BackendServer _selectedBackendServer = BackendServer.Local;

    protected override async Task OnInitializedAsync()
    {
        _selectedBackendServer = await localStorageService.GetItemAsync<BackendServer?>("SelectedServer") ?? BackendServer.Local;
    }

    private async Task AskAi()
    {
        if (string.IsNullOrWhiteSpace(_question))
        {
            return;
        }

        AIAgent agentToUse = _selectedPersona switch
        {
            ChatPersona.ComicBookGuy => GetComicBookAgent(),
            ChatPersona.Assistant => GetAssistantAgent(),
            _ => throw new ArgumentOutOfRangeException()
        };

        _answer = string.Empty;
        await foreach (AgentRunResponseUpdate update in agentToUse.RunStreamingAsync(_question))
        {
            _answer += update.Text;
            StateHasChanged();
        }
    }

    private AIAgent GetComicBookAgent()
    {
        return new AGUIChatClient(httpClient, $"{GetBackEndUrl()}/comic-book-guy").CreateAIAgent();
    }

    private AIAgent GetAssistantAgent()
    {
        return new AGUIChatClient(httpClient, $"{GetBackEndUrl()}/assistant").CreateAIAgent();
    }

    private string GetBackEndUrl()
    {
        return _selectedBackendServer switch
        {
            BackendServer.Local => "https://localhost:7002",
            BackendServer.WebAppWindows => "https://best-comic-book-store-ever-backend.azurewebsites.net",
            BackendServer.WebAppLinux => "https://best-comic-book-store-ever-backend-linux.azurewebsites.net",
            BackendServer.ContainerAppsLinux => "https://book-store-backend-container.lemondesert-e9767820.swedencentral.azurecontainerapps.io",
            BackendServer.InternetInformationServer => "http://localhost:8080",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void SetPersona(ChatPersona persona)
    {
        _selectedPersona = persona;
    }

    private string GetPersonaButtonClass(ChatPersona persona) =>
        persona == _selectedPersona ? "persona-button active" : "persona-button";

    private enum ChatPersona
    {
        ComicBookGuy,
        Assistant
    }

    private enum BackendServer
    {
        Local = 0,
        WebAppWindows = 1,
        WebAppLinux = 2,
        ContainerAppsLinux = 3,
        InternetInformationServer = 4,
    }

    private async Task OnBackendRootUrlChanged()
    {
        await localStorageService.SetItemAsync("SelectedServer", _selectedBackendServer);
    }
}