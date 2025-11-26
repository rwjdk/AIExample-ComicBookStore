using BlazorWasm.FrontEnd.Pages;

namespace SharedModels;

public class AnswerRequest
{
    public required ChatPersona Persona { get; set; }
    public required string Question { get; set; }
}