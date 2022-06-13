﻿// <auto-generated />
using System;
using AudioStreaming.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AudioStreaming.Persistence.Migrations
{
    [DbContext(typeof(AudioStreamingContext))]
    [Migration("20220612234756_change-chart6")]
    partial class changechart6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Artist", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileImageUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pseudonym")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Artist");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Chart", b =>
                {
                    b.Property<int>("Position")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Position"), 1L, 1);

                    b.HasKey("Position");

                    b.ToTable("Chart");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Genre", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Name");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.ListeningHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TrackId");

                    b.HasIndex("UserId");

                    b.ToTable("ListeningHistory");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("PlaylistCoverUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Playlist");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.PlaylistPermission", b =>
                {
                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("PlaylistId", "UserId", "Type");

                    b.HasIndex("UserId");

                    b.ToTable("PlaylistPermission");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset?>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("ReleaseCoverUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Stage")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Release");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.ReleaseParticipant", b =>
                {
                    b.Property<int>("ReleaseId")
                        .HasColumnType("int");

                    b.Property<string>("ArtistId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte>("Order")
                        .HasColumnType("tinyint");

                    b.HasKey("ReleaseId", "ArtistId");

                    b.HasIndex("ArtistId");

                    b.ToTable("ReleaseParticipant");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.ReleaseVerificationHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("NewStage")
                        .HasColumnType("int");

                    b.Property<int>("ReleaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReleaseId");

                    b.ToTable("ReleaseVerificationHistory");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PathInStorage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PositionInChart")
                        .HasColumnType("int");

                    b.Property<int>("ReleaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PositionInChart")
                        .IsUnique()
                        .HasFilter("[PositionInChart] IS NOT NULL");

                    b.HasIndex("ReleaseId");

                    b.ToTable("Track");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.TrackParticipant", b =>
                {
                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.Property<string>("ArtistId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte>("Order")
                        .HasColumnType("tinyint");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("TrackId", "ArtistId");

                    b.HasIndex("ArtistId");

                    b.ToTable("TrackParticipant");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("RefreshTokenExperation")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("GenreTrack", b =>
                {
                    b.Property<string>("GenresName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TracksId")
                        .HasColumnType("int");

                    b.HasKey("GenresName", "TracksId");

                    b.HasIndex("TracksId");

                    b.ToTable("GenreTrack");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PlaylistTrack", b =>
                {
                    b.Property<int>("PlaylistsId")
                        .HasColumnType("int");

                    b.Property<int>("TracksId")
                        .HasColumnType("int");

                    b.HasKey("PlaylistsId", "TracksId");

                    b.HasIndex("TracksId");

                    b.ToTable("PlaylistTrack");
                });

            modelBuilder.Entity("PlaylistUser", b =>
                {
                    b.Property<int>("FavoritePlaylistsId")
                        .HasColumnType("int");

                    b.Property<string>("UsersWhoAddedToFavoriteId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FavoritePlaylistsId", "UsersWhoAddedToFavoriteId");

                    b.HasIndex("UsersWhoAddedToFavoriteId");

                    b.ToTable("PlaylistUser");
                });

            modelBuilder.Entity("ReleaseUser", b =>
                {
                    b.Property<int>("FavoriteReleasesId")
                        .HasColumnType("int");

                    b.Property<string>("UsersWhoAddedToFavoriteId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FavoriteReleasesId", "UsersWhoAddedToFavoriteId");

                    b.HasIndex("UsersWhoAddedToFavoriteId");

                    b.ToTable("ReleaseUser");
                });

            modelBuilder.Entity("TrackUser", b =>
                {
                    b.Property<int>("FavoriteTracksId")
                        .HasColumnType("int");

                    b.Property<string>("UsersWhoAddedToFavoriteId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FavoriteTracksId", "UsersWhoAddedToFavoriteId");

                    b.HasIndex("UsersWhoAddedToFavoriteId");

                    b.ToTable("TrackUser");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Artist", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.User", "User")
                        .WithOne("Artist")
                        .HasForeignKey("AudioStreaming.Domain.Entities.Artist", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.ListeningHistory", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Track", "Track")
                        .WithMany("ListeningHistory")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.User", "User")
                        .WithMany("ListeningHistory")
                        .HasForeignKey("UserId");

                    b.Navigation("Track");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.PlaylistPermission", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Playlist", "Playlist")
                        .WithMany("Permissions")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.ReleaseParticipant", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Artist", "Artist")
                        .WithMany("ParticipatingInReleases")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.Release", "Release")
                        .WithMany("Participants")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Release");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.ReleaseVerificationHistory", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Release", "Release")
                        .WithMany("VerificationHistory")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Release");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Track", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Chart", null)
                        .WithOne("Track")
                        .HasForeignKey("AudioStreaming.Domain.Entities.Track", "PositionInChart");

                    b.HasOne("AudioStreaming.Domain.Entities.Release", "Release")
                        .WithMany("Tracks")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Release");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.TrackParticipant", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Artist", "Artist")
                        .WithMany("ParticipatingInTracks")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.Track", "Track")
                        .WithMany("Participants")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("GenreTrack", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.Track", null)
                        .WithMany()
                        .HasForeignKey("TracksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlaylistTrack", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Playlist", null)
                        .WithMany()
                        .HasForeignKey("PlaylistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.Track", null)
                        .WithMany()
                        .HasForeignKey("TracksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlaylistUser", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Playlist", null)
                        .WithMany()
                        .HasForeignKey("FavoritePlaylistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWhoAddedToFavoriteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReleaseUser", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Release", null)
                        .WithMany()
                        .HasForeignKey("FavoriteReleasesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWhoAddedToFavoriteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrackUser", b =>
                {
                    b.HasOne("AudioStreaming.Domain.Entities.Track", null)
                        .WithMany()
                        .HasForeignKey("FavoriteTracksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioStreaming.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWhoAddedToFavoriteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Artist", b =>
                {
                    b.Navigation("ParticipatingInReleases");

                    b.Navigation("ParticipatingInTracks");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Chart", b =>
                {
                    b.Navigation("Track");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Playlist", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Release", b =>
                {
                    b.Navigation("Participants");

                    b.Navigation("Tracks");

                    b.Navigation("VerificationHistory");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.Track", b =>
                {
                    b.Navigation("ListeningHistory");

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("AudioStreaming.Domain.Entities.User", b =>
                {
                    b.Navigation("Artist");

                    b.Navigation("ListeningHistory");

                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
