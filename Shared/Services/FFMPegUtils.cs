using FFMpegCore;

namespace Shared.Services
{
    public class FFMPegUtils
    {
        public async Task<bool> TryExtractAudioAsync(string videoPath)
        {
            
            string tempAudioPath = videoPath.Replace(".mp4", "_temp.mp3");

            if(!FFMpeg.ExtractAudio(videoPath, tempAudioPath))
            {
                return false;
            }
             
            var finalAudioPath = videoPath.Replace(".mp4", ".mp3");
            if(!await ReduceSizeAsync(tempAudioPath, finalAudioPath))
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
    }
}
