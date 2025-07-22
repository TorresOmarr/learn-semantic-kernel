

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Spectre.Console;
using System.ComponentModel;
using System.Xml;

IChatCompletionService _chatCompletionService;
ChatHistory _chatHistory;
OpenAIPromptExecutionSettings _executionSettings;

InitializeKernels();
ShowWelcomeMessage();

await RunChatLoopAsync();



async Task<ChatMessageContentItemCollection> CreateUserContentAsync(string additionalText, string imagePathOrUrl)
{
    var contents = new ChatMessageContentItemCollection();

    if (!string.IsNullOrWhiteSpace(additionalText))
    {
        contents.Add(new TextContent(additionalText));
    }
    else
    {
        contents.Add(new TextContent("Give me the description of the image"));
    }


    if (imagePathOrUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
        imagePathOrUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
    {
        contents.Add(new ImageContent(new Uri(imagePathOrUrl)));
    }
    else
    {
        AnsiConsole.MarkupLine($"[grey]Reading image from local path...[/]");
        if (!File.Exists(imagePathOrUrl))
        {
            AnsiConsole.MarkupLine($"[red]The image path '{imagePathOrUrl}' does not exist.[/]");
            return null;
        }
        try
        {
            var imageBytes = File.ReadAllBytes(imagePathOrUrl);
            var mimeType = InferMimeType(imagePathOrUrl);
            contents.Add(new ImageContent(imageBytes, mimeType));
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error reading the image: {ex.Message}[/]");
            return null;
        }

    }

    return contents;
}

string InferMimeType(string filePath)
{
    var extension = Path.GetExtension(filePath).ToLowerInvariant();
    return extension switch
    {
        ".png" => "image/png",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".gif" => "image/gif",
        _ => "image/jpeg" // Default
    };
}

void InitializeKernels()
{
    string? openAIKey = Environment.GetEnvironmentVariable("SkCourseOpenAIKey");
    string? azureRegion = Environment.GetEnvironmentVariable("SkCourseAzureRegion");
    string? azureKey = Environment.GetEnvironmentVariable("SkCourseAzureKey");


    Kernel openAIKernel = Kernel.CreateBuilder()
                                    .AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}")
                                    .Build();

    Kernel azureKernel = Kernel.CreateBuilder()
                                    .AddAzureOpenAIChatCompletion(deploymentName: "gpt-4o-mini",
                                                                  endpoint: $"{azureRegion}",
                                                                  apiKey: $"{azureKey}")
                                    .Build();
    _executionSettings = new OpenAIPromptExecutionSettings()
    {
        MaxTokens = 4000,
        Temperature = 0.7,
    };
    _chatCompletionService = openAIKernel.GetRequiredService<IChatCompletionService>();
    _chatHistory = new ChatHistory("You are a helpful assistant ");
}

void ShowWelcomeMessage()
{
    AnsiConsole.MarkupLine("[bold green] Welcome to semantic Kernel Chat!![/]");
    AnsiConsole.MarkupLine("Options");
    AnsiConsole.MarkupLine(" - Type text and press Enter");
    AnsiConsole.MarkupLine(" - Type [blue]img[/] to attach an image (local path o URL)");
    AnsiConsole.MarkupLine(" - Type [red]exit[/] to terminate");
}




async Task RunChatLoopAsync()
{
    while (true)
    {
        var userInput = AnsiConsole.Ask<string>("[blue]User:[/]");

        if (string.IsNullOrEmpty(userInput))
        {
            break;
        }

        if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            AnsiConsole.MarkupLine("[bold red]Exiting the chat...[/]");
            break;
        }

        if (userInput.Equals("img", StringComparison.OrdinalIgnoreCase))
        {
            var imagePathOrUrl = AnsiConsole.Prompt(
                    new TextPrompt<string>("[blue]Enter the image path (local) or URL:[/]")
                     .Validate(pathOrUrl =>
                     {
                         if (string.IsNullOrWhiteSpace(pathOrUrl))
                         {
                             return ValidationResult.Error("[red]The image path or Url cannot be empty.");
                         }

                         return ValidationResult.Success();
                     })
                );

            AnsiConsole.MarkupLine($"Do you want to add additional descriptive text?");
            var additionalText =
                      AnsiConsole.Ask<string>("[blue](Leave empty if you don't want to add text):[/]");

            var userMessageContents = await CreateUserContentAsync(additionalText, imagePathOrUrl);
            if (userMessageContents is null)
            {
                continue;
            }
            _chatHistory.AddUserMessage(userMessageContents);

        }
        else
        {
            _chatHistory.AddUserMessage(userInput);
        }

        try
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Balloon)
                .StartAsync("Thinking...", async ctx =>
                {
                    var response = await _chatCompletionService.GetChatMessageContentAsync(_chatHistory, _executionSettings);

                    _chatHistory.AddAssistantMessage(response.ToString());
                });

            AnsiConsole.MarkupLine($"[bold green]Assistant:[/] {_chatHistory.LastOrDefault()}\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message}");
        }


    }
}