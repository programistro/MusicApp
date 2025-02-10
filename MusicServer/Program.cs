using Microsoft.AspNetCore.Http.Features;
using MusicServer;
using MusicServer.Data;
using MusicServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddSingleton<IAudioFileProvider>(
    new AudioFileProvider(@"wav\"));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100 MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
});

builder.Services.AddSwaggerGen();

// Регистрируем сервис
builder.Services.AddSingleton<AudioStreamService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapGrpcService<AudioStreamService>();
app.MapControllers();
app.Run();