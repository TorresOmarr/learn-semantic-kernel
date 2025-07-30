using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AudioToText;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace Shared.Services
{
    public class WhisperTranscriptionService
    {
        public async Task<string> TranscribeAudioAsync(string audioPath, string language, Kernel kernel)
        {
            var srtPath = audioPath.Replace(".mp3", ".srt");
            if (File.Exists(srtPath))
            {
                var content = File.ReadAllText(srtPath);
                return content;
            }

            var audioToTextService = kernel.GetRequiredService<IAudioToTextService>();

            var executionSettings = new OpenAIAudioToTextExecutionSettings()
            {
                Language = language,
                Prompt = "This is a prompt. You should respect punctuation marks",
                ResponseFormat = "srt"
            };

            var audioFileStream = File.OpenRead(audioPath);
            var audioFileBinaryData = await BinaryData.FromStreamAsync(audioFileStream);

            AudioContent audioContent = new AudioContent(audioFileBinaryData, null);

            var textContent = await audioToTextService.GetTextContentAsync(
                audioContent,
                executionSettings,
                kernel
            );

            var srtFilePath = audioPath.Replace(".mp3", ".srt");
            await File.WriteAllTextAsync(srtFilePath, textContent.Text);

            return textContent.Text ?? "";

        }
    }
}
