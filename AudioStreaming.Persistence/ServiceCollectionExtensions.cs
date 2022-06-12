using AudioStreaming.Application.Abstractions.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioStreaming.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AudioStreamingContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"), 
                    b => b.MigrationsAssembly(typeof(AudioStreamingContext).Assembly.ToString())));

            services.AddScoped<IAudioStreamingContext>(provider => provider.GetService<AudioStreamingContext>());
        }
    }
}
