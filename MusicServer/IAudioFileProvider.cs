using GrpcAudioStreaming;
using NAudio.Wave;

namespace MusicServer;

public interface IAudioFileProvider : IDisposable
{
    Task<IEnumerable<string>> GetAvailableFilesAsync();
    Task<AudioFormat> GetAudioFormatAsync(string fileName);
    Task<IAudioSampleSource> CreateAudioSampleSourceAsync(string fileName);
}

public class AudioFileProvider : IAudioFileProvider
{
    private readonly string _audioDirectory;
    
    public AudioFileProvider(string audioDirectory)
    {
        _audioDirectory = audioDirectory;
    }

    public async Task<IEnumerable<string>> GetAvailableFilesAsync()
    {
        var files = Directory.GetFiles(_audioDirectory, "*.wav");
        return files.Select(Path.GetFileName);
    }

    public async Task<AudioFormat> GetAudioFormatAsync(string fileName)
    {
        using var fileReader = new WaveFileReader(
            Path.Combine(_audioDirectory, fileName));
        
        return fileReader.WaveFormat.ToAudioFormat();
    }

    public async Task<IAudioSampleSource> CreateAudioSampleSourceAsync(string fileName)
    {
        return new AudioSampleSource(
            Path.Combine(_audioDirectory, fileName));
    }

    public void Dispose()
    {
        // Освобождение ресурсов если нужно
    }
}