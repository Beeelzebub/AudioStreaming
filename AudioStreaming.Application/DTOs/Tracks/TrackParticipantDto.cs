﻿using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Tracks
{
    public class TrackParticipantDto
    {
        public string ArtistId { get; set; }

        public string Pseudonym { get; set; }

        public ParticipantRole Role { get; set; }

        public byte Order { get; set; }
    }
}
