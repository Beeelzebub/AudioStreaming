using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AudioStreaming.ChartUpdater
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddDbContext<AudioStreamingContext>(options =>
                options.UseSqlServer(configuration["SqlConnectionString"]));

            builder.Services.AddScoped<IAudioStreamingContext, AudioStreamingContext>(provider => 
                provider.GetService<AudioStreamingContext>());
        }
    }
}
