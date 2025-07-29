#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
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
            string? openAIKey = Environment.GetEnvironmentVariable("SkCourseOpenAIKey");
            string? azureRegion = Environment.GetEnvironmentVariable("SkCourseAzureRegion");
            string? azureKey = Environment.GetEnvironmentVariable("SkCourseAzureKey");

            if (type == TypeKernel.OpenAI)
            {                
                if(string.IsNullOrEmpty(openAIKey))
                {
                    throw new InvalidOperationException("OpenAI API key is not set in the environment variables.");
                }

                builder.AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}")
                        .AddOpenAIAudioToText("whisper-1",apiKey: $"{openAIKey}");  

                builder.Services.AddSingleton<FFMPegUtils>();
                builder.Plugins.AddFromType<VideoPlugin>();

            }
            else if (type == TypeKernel.AzureOpenAI)
            {             
                if(string.IsNullOrEmpty(azureRegion) || string.IsNullOrEmpty(azureKey))
                {
                    throw new InvalidOperationException("Azure OpenAI API region or key is not set in the environment variables.");
                }

                builder.AddAzureOpenAIChatCompletion(deploymentName: "gpt-4o-mini",
                                                                  endpoint: $"{azureRegion}",
                                                                  apiKey: $"{azureKey}");
                builder.Services.AddSingleton<FFMPegUtils>();
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