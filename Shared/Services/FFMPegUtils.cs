using FFMpegCore;
using Microsoft.SemanticKernel;

namespace Shared.Services
{
    public class FFMPegUtils
    {
        public async Task<bool> TryExtractAudioAsync(string videoPath)
        {

            string tempAudioPath = videoPath.Replace(".mp4", "_temp.mp3");

            if (!FFMpeg.ExtractAudio(videoPath, tempAudioPath))
            {
                return false;
            }

            var finalAudioPath = videoPath.Replace(".mp4", ".mp3");
            if (!await ReduceSizeAsync(tempAudioPath, finalAudioPath))
            {
                return false;
            }

            File.Delete(tempAudioPath);

            return true;


        }

        public async Task<bool> ReduceSizeAsync(string inputFile, string outputFile)
        {
            return await FFMpegArguments.FromFileInput(inputFile)
                .OutputToFile(outputFile, overwrite: true, options =>
                    options.WithCustomArgument("-ar 16000 -ac 1 -b:a 32k"))
                .ProcessAsynchronously();
        }

        public async Task<string> CutVideo(string inputPath, string newVideoName, TimeSpan startTime,
            TimeSpan endTime, Kernel kernel)
        {
            var outputPath = Path.Combine(Path.GetDirectoryName(inputPath), newVideoName);
            string ffmpegStartTime = startTime.ToString(@"hh\:mm\:ss");
            string ffmpegEndTime = endTime.ToString(@"hh\:mm\:ss");

            bool result = await FFMpegArguments
                               .FromFileInput(inputPath)
                               .OutputToFile(outputPath, overwrite: true, options => options.WithCustomArgument
                               ($"-ss {ffmpegStartTime} -to {ffmpegEndTime}"))
                               .ProcessAsynchronously();

            if (!result)
            {
                return "Failed to cut video. Please check the input parameters.";
            }

            return $"Video cut successfully at {outputPath}";
        }
    }
}
