


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Shared;
using Spectre.Console;



Kernel openAIKernel = KernelFactory.CreateKernel(TypeKernel.OpenAI);
Kernel azureKernel = KernelFactory.CreateKernel(TypeKernel.AzureOpenAI);


var chatCompletionService = openAIKernel.GetRequiredService<IChatCompletionService>();



OpenAIPromptExecutionSettings executionSettings = new OpenAIPromptExecutionSettings()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};


ChatHistory chatHistory = new();

string? input = null;

chatHistory.AddAssistantMessage("Hi! How would you like to get started? How about trying to get the transcript of your video by providing the path to the file?");

AnsiConsole.MarkupLine($"[bold green] Assistant: [/][italic]{chatHistory.LastOrDefault()}[/]");

while (true)
{
    AnsiConsole.Markup("[bold blue]User: [/]");
    input = Console.ReadLine()?.Trim();

    if(string.IsNullOrWhiteSpace(input))
    {
        break;
    }
    chatHistory.AddUserMessage(input);
    var chatResult = await chatCompletionService.GetChatMessageContentAsync(
        chatHistory,
        executionSettings,
        openAIKernel
    );

    chatHistory.AddAssistantMessage(chatResult.ToString());

    AnsiConsole.MarkupLine($"[bold green] Assistant: [/][italic]{chatHistory.LastOrDefault()}[/]");

}

