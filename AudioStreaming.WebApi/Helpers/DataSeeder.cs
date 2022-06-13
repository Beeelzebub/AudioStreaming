using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Security;

namespace AudioStreaming.WebApi.Helpers
{
    public static class DataSeeder
    {
        private const string ReleaseCoverLocalPath = "D:\\Desktop\\Учебыч\\8 сем ВГУ\\Диплом\\Art-history-lectures-cover.jpg";
        private const string TracksLocalDirectoryPath = "D:\\Desktop\\Учебыч\\8 сем ВГУ\\Диплом\\Art-history-lectures";

        public static async Task SeedDataAsync(IAudioStreamingContext dbContext, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            ITrackBlobService trackBlobService,
            ICoverBlobService coverBlobService)
        {
            if (await roleManager.FindByNameAsync(AudioStreamingRoles.Artist) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(AudioStreamingRoles.Artist));
            }
            if (await roleManager.FindByNameAsync(AudioStreamingRoles.Moderator) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(AudioStreamingRoles.Moderator));
            }
            if (await userManager.FindByNameAsync("moder") == null)
            {
                var moder = new User { UserName = "moder" };

                IdentityResult result = await userManager.CreateAsync(moder, "moder");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(moder, AudioStreamingRoles.Moderator);
                }
            }
            if (await userManager.FindByNameAsync("artist") == null)
            {
                var user = new User { UserName = "artist" };

                var artist = new Artist
                {
                    IsConfirmed = true,
                    Pseudonym = "Aksenova Alina",
                    Email = "somemail@gmail.com",
                    Country = "Russia",
                    Description = "Art history teacher"
                };

                user.Artist = artist;

                IdentityResult result = await userManager.CreateAsync(user, "artist");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AudioStreamingRoles.Artist);
                }
            }

            var chart = new List<Chart>();

            for (int i = 1; i <= 100; i++)
            {
                chart.Add(new Chart());
            }

            await dbContext.Chart.AddRangeAsync(chart);

            var genres = new List<Genre>
            {
                new Genre { Name = "Lecture" },
                new Genre { Name = "Rock" },
                new Genre { Name = "Hip-Hop" },
                new Genre { Name = "Pop" },
                new Genre { Name = "Alternative" },
                new Genre { Name = "Metall" }
            };

            await dbContext.Genre.AddRangeAsync(genres);

            var releaseArtist = dbContext.User.Include(u => u.Artist).SingleOrDefault(u => u.UserName == "artist");

            var release = new Release
            {
                Date = DateTimeOffset.UtcNow,
                Stage = ReleaseStage.Published,
                Title = "art history lectures",
                Description = "art history lectures by Aksenova Alina",
                Type = ReleaseType.Album,
                Participants = new List<ReleaseParticipant> { new ReleaseParticipant { ArtistId = releaseArtist.Id } }
            };

            using (var coverStream = new FileStream(ReleaseCoverLocalPath, FileMode.Open))
            {
                release.ReleaseCoverUri = await coverBlobService.UploadReleaseCoverAsync(1, coverStream);
            }

            await dbContext.Release.AddAsync(release);

            await dbContext.SaveChangesAsync();

            int id = 1;

            foreach (string trackLocalPath in Directory.EnumerateFiles(TracksLocalDirectoryPath, "*.mp3"))
            {
                var track = new Track
                {
                    Name = trackLocalPath.Split('\\').Last(),
                    Release = release,
                    Genres = new List<Genre> { genres.FirstOrDefault(g => g.Name == "Lecture") },
                    Participants = new List<TrackParticipant> { new TrackParticipant { ArtistId = releaseArtist.Id, Role = ParticipantRole.Author, Order = 1 } }
                };

                using (var trackStream = new FileStream(trackLocalPath, FileMode.Open))
                {
                    track.PathInStorage = await trackBlobService.UploadAsync(release.Id, track.Name, trackStream);
                }

                await dbContext.Track.AddAsync(track);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
