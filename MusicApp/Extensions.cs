using GrpcAudioStreaming;
using NAudio.Wave;

namespace MusicApp;

public static class Extensions
{
    public static WaveFormat ToWaveFormat(this AudioFormat audioFormat)
    {
        return WaveFormat.CreateCustomFormat(
            (WaveFormatEncoding)Enum.Parse(typeof(WaveFormatEncoding), audioFormat.Encoding),
            audioFormat.SampleRate,
            audioFormat.Channels,
            audioFormat.AverageBytesPerSecond,
            audioFormat.BlockAlign,
            audioFormat.BitsPerSample);
    }
}