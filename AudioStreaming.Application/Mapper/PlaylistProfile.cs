﻿using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mapper.Converters;
using AudioStreaming.Domain.Entities;
using AutoMapper;

namespace AudioStreaming.Application.Mapper
{
    public class PlaylistProfile : Profile
    {
        public PlaylistProfile()
        {
            CreateMap<Playlist, PlaylistDto>();

            CreateMap<PagedList<Playlist>, PagedList<PlaylistDto>>()
                .ConvertUsing<PagedListConverter<Playlist, PlaylistDto>>(); ;
        }
    }
}
