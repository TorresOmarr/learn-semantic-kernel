using Microsoft.SemanticKernel;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Plugins
{
    public class SpeechToTextPlugin
    {
        private readonly WhisperTranscriptionService _whisperTranscriptionService;

        public SpeechToTextPlugin(WhisperTranscriptionService whisperTranscriptionService)
        {
            _whisperTranscriptionService = whisperTranscriptionService;
        }

        [KernelFunction]
        [Description("Transcribe mp3 files using the whisper service. This methos should only process .mp3 files, not video files")]
        public async Task<string> GetTranscription(string filePath, string language, Kernel kernel)
        {
            var transcript = await _whisperTranscriptionService.TranscribeAudioAsync(filePath, language, kernel);

            return transcript;
        }
    }
}
