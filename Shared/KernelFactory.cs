using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Shared.Plugins;
using Shared.Services;

namespace Shared
{
    public static class KernelFactory
    {
        public static Kernel CreateKernel(TypeKernel type)
        {
            var builder = Kernel.CreateBuilder();

            builder.Services.AddSingleton<IFileService, FileService>();
         
            if (type == TypeKernel.OpenAI)
            {
                string? openAIKey = Environment.GetEnvironmentVariable("SkCourseOpenAIKey");
                builder.AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}");

            }
            else if (type == TypeKernel.AzureOpenAI)
            {
                string? azureRegion = Environment.GetEnvironmentVariable("SkCourseAzureRegion");
                string? azureKey = Environment.GetEnvironmentVariable("SkCourseAzureKey");
                builder.AddAzureOpenAIChatCompletion(deploymentName: "gpt-4o-mini",
                                                                  endpoint: $"{azureRegion}",
                                                                  apiKey: $"{azureKey}");
            }
            builder.Plugins.AddFromType<FilePlugin>();
            return builder.Build();
        }
    }

    public enum TypeKernel
    {
        AzureOpenAI,
        OpenAI
    }
}