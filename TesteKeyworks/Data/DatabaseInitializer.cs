using TesteKeyworks.Models;
using TesteKeyworks.Services.Streamings;

namespace TesteKeyworks.Data
{
    public class DatabaseInitializer
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DatabaseInitializer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InitializeAsync()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var streamingService = scope.ServiceProvider.GetRequiredService<IStreamingService>();
                    var streaming = await streamingService.GetByNomeAsync("FilmeFlix");
                    if (streaming == null || !streaming.Any())
                    {
                        var filmeFlixStreaming = new Streaming
                        {
                            Nome = "FilmeFlix",
                            Filmes = new List<Filme>()
                        };

                        await streamingService.AddAsync(filmeFlixStreaming);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
