using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Net.Client;
using GrpcAudioStreaming;
using GrpcAudioStreaming.Client;

namespace MusicClient.Test;

class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5001");
        var client = new AudioStream.AudioStreamClient(channel);
        Console.WriteLine("Press any key to exit...");
// Получение списка доступных файлов
        var filesResponse = client.GetAvailableFiles(new Empty());
        var availableFiles = await filesResponse.ResponseStream.ToListAsync();

// Выбор файла и получение его формата
        foreach (var file in availableFiles)
        {
            Console.WriteLine(file);
        }

        // var selectedFile = availableFiles.First();
        var selectedFile = Console.ReadLine();
        var format = client.GetFormat(new FileName { FileName_ = selectedFile });

        using var audioPlayer = new AudioPlayer(format.ToWaveFormat());
        audioPlayer.Play();
        
// Запуск воспроизведения выбранного файла
        var streamResponse = client.GetStream(new FileName { FileName_ = selectedFile });
        await foreach (var sample in streamResponse.ResponseStream.ReadAllAsync())
        {
            audioPlayer.AddSample(sample.Data.ToByteArray());
        }
    }
}