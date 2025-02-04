using MusicServer;
using MusicServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

// Регистрируем провайдер файлов
builder.Services.AddSingleton<IAudioFileProvider>(
    new AudioFileProvider(@"wav\"));

// Регистрируем сервис
builder.Services.AddSingleton<AudioStreamService>();

var app = builder.Build();
app.UseRouting();
app.MapGrpcService<AudioStreamService>();
app.Run();