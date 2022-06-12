using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Application.Abstractions.Services.Releases;
using AudioStreaming.Infrastructure.BackgroundJobs;
using AudioStreaming.Infrastructure.Services.BlobStorage;
using AudioStreaming.Infrastructure.Services.Releases;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioStreaming.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                //.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration["Hangfire:HangfireStorageString"]));

            services.AddHangfireServer();

            services.AddScoped<ICoverBlobService, CoverBlobService>();
            services.AddScoped<ITrackBlobService, TrackBlobService>();
            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<ReleasePublisher>();
            services.AddScoped<ChartUpdater>();
        }
    }
}
