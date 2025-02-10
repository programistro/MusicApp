using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Net.Client;
using GrpcAudioStreaming;
using GrpcAudioStreaming.Client;

namespace MusicClient.Test;

class Program
{
    private static string url = "https://localhost:7218/";
    
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Если хотите загрузить файл введите 1, если хотите прослушть аудио ввеидите 2, если хотите сменить описание то ввеидите 3");
            var line = Console.ReadLine();

            if (line == "1")
            {
                Console.WriteLine("Введите путь к файлу(.wav)");
                string filePath = Console.ReadLine();

                using (HttpClient client = new HttpClient(new HttpClientHandler
                       {
                           SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                          System.Security.Authentication.SslProtocols.Tls13,
                           MaxConnectionsPerServer = 10
                       }))
                {
                    client.DefaultRequestVersion = HttpVersion.Version20;
                    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;

                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        using (FileStream fileStream = File.OpenRead(filePath))
                        {
                            StreamContent fileContent = new StreamContent(fileStream);

                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                            content.Add(fileContent, "file", Path.GetFileName(filePath));

                            HttpResponseMessage response = await client.PostAsync($"{url}api/upload-file", content);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Файл успешно загружен.");
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: {response.StatusCode}");
                                string responseContent = await response.Content.ReadAsStringAsync();
                                Console.WriteLine(responseContent);
                            }
                        }
                    }
                }
            }
            else if (line == "2")
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7218");
                var client = new AudioStream.AudioStreamClient(channel);
                var filesResponse = client.GetAvailableFiles(new Empty());
                var availableFiles = await filesResponse.ResponseStream.ToListAsync();

                foreach (var file in availableFiles)
                {
                    Console.WriteLine(file);
                }
                var selectedFile = Console.ReadLine();
                var format = client.GetFormat(new FileName { FileName_ = selectedFile });

                using var audioPlayer = new AudioPlayer(format.ToWaveFormat());
                audioPlayer.Play();
        
                var streamResponse = client.GetStream(new FileName { FileName_ = selectedFile });
                await foreach (var sample in streamResponse.ResponseStream.ReadAllAsync())
                {
                    audioPlayer.AddSample(sample.Data.ToByteArray());

                    Console.WriteLine("play");
                }
            }
            else if (line == "3")
            {
                using (HttpClient client = new HttpClient(new HttpClientHandler
                       {
                           SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                          System.Security.Authentication.SslProtocols.Tls13,
                           MaxConnectionsPerServer = 10
                       }))
                {
                    client.DefaultRequestVersion = HttpVersion.Version20;
                    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
                    
                    try
                    {
                        Console.WriteLine("Введите название файла");
                        string fileName = Console.ReadLine();
                        Console.WriteLine("Введите описание к файлу");
                        string description = Console.ReadLine();

                        var response = await client.PutAsync($"{url}api/create-description-file?fileName={fileName}&description={description}", null);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Запрос успешно выполнен!");
                            var responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Ответ сервера: {responseBody}");
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    }   
                }
            }
        }
    } 
}