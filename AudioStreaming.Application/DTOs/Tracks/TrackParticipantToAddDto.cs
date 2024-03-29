﻿using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Tracks
{
    public class TrackParticipantToAddDto
    {
        public string ArtistId { get; set; }

        public ParticipantRole Role { get; set; }

        public byte Order { get; set; }
    }
}
