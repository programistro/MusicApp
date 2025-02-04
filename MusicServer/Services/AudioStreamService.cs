using System.Collections.Concurrent;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcAudioStreaming;

namespace MusicServer.Services
{
    public class AudioStreamService : AudioStream.AudioStreamBase
    {
        private readonly IAudioFileProvider _audioFileProvider;
        private readonly Dictionary<string, IAudioSampleSource> _activeStreams 
            = new Dictionary<string, IAudioSampleSource>();
    
        public AudioStreamService(IAudioFileProvider audioFileProvider)
        {
            _audioFileProvider = audioFileProvider;
        }

        public override async Task GetAvailableFiles(Empty request, 
            IServerStreamWriter<FileName> responseStream, 
            ServerCallContext context)
        {
            var files = await _audioFileProvider.GetAvailableFilesAsync();
            foreach (var file in files)
            {
                await responseStream.WriteAsync(new FileName { FileName_ = file });
            }
        }

        public override async Task<AudioFormat> GetFormat(FileName request, 
            ServerCallContext context)
        {
            return await _audioFileProvider.GetAudioFormatAsync(request.FileName_);
        }

        public override async Task GetStream(FileName request, 
            IServerStreamWriter<AudioSample> responseStream, 
            ServerCallContext context)
        {
            var audioSource = await _audioFileProvider.CreateAudioSampleSourceAsync(
                request.FileName_);

            _activeStreams[request.FileName_] = audioSource;

            audioSource.AudioSampleCreated += async (_, audioSample) => 
                await responseStream.WriteAsync(audioSample);

            try
            {
                await audioSource.StartStreaming();
            }
            finally
            {
                if (_activeStreams.TryGetValue(request.FileName_, out var source))
                {
                    source.StopStreaming();
                    _activeStreams.Remove(request.FileName_);
                }
            }
        }
    }
}