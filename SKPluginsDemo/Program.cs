using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Shared;

IChatCompletionService _chatCompletionService;
ChatHistory _chatHistory;
OpenAIPromptExecutionSettings _executionSettings;

InitializeKernels();

void InitializeKernels()
{
    Kernel openAIKernel = KernelFactory.CreateKernel(TypeKernel.OpenAI);
    Kernel azureKernel = KernelFactory.CreateKernel(TypeKernel.AzureOpenAI);

    _executionSettings = new OpenAIPromptExecutionSettings()
    {
        MaxTokens = 4000,
        Temperature = 0.7,
    };
    _chatCompletionService = openAIKernel.GetRequiredService<IChatCompletionService>();
    _chatHistory = new ChatHistory("You are a helpful assistant ");
}