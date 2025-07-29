using FFMpegCore;

namespace Shared.Services
{
    public class FFMPegUtils
    {
        public bool TryExtractAudio(string videoPath)
        {
            string tempAudioPath = videoPath.Replace(".mp4", "_temp.mp3");
            return FFMpeg.ExtractAudio(videoPath, tempAudioPath);
        }
    }
}
