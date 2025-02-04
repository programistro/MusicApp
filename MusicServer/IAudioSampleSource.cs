using GrpcAudioStreaming;

namespace MusicServer;

public interface IAudioSampleSource
{
    event EventHandler<AudioSample> AudioSampleCreated;

    AudioFormat AudioFormat { get; }

    Task StartStreaming();
    void StopStreaming();
}
