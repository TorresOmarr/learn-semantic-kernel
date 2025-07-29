
using Microsoft.SemanticKernel;
using Shared.Services;

namespace Shared.Plugins
{
    public class VideoPlugin
    {
        private readonly FFMPegUtils _ffmpegUtils;
        public VideoPlugin(FFMPegUtils ffmpegUtils)
        {
            _ffmpegUtils = ffmpegUtils;
        }

        [KernelFunction]
        public async Task<string> ExtractAudioFromVideo(string inputPath)
        {
            if(!File.Exists(inputPath))
            {
                return "The file does not exist.";
            }


            if (!await _ffmpegUtils.TryExtractAudioAsync(inputPath))
            {
                return "Failed to extract audio";
            }

            return $"Audio extracted successfully at {inputPath.Replace(".mp4", ".mp3")}";

        }

    }
}
