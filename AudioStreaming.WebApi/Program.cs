using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Domain.Entities;
using AudioStreaming.WebApi.Helpers;
using Microsoft.AspNetCore.Identity;

namespace AudioStreaming.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await SeedData(host.Services);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task SeedData(IServiceProvider serviceprovider)
        {
            using (var scope = serviceprovider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var dbContext = services.GetRequiredService<IAudioStreamingContext>();
                    var coverBlobService = services.GetRequiredService<ICoverBlobService>();
                    var trackBlobService = services.GetRequiredService<ITrackBlobService>();

                    await DataSeeder.SeedDataAsync(dbContext, userManager, rolesManager, trackBlobService, coverBlobService);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }

}