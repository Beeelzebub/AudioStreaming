using AutoMapper;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.DTOs.Tracks;
using AudioStreaming.Application.Mapper.Converters;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Application.Mapper
{
    public class TrackProfile : Profile
    {
        public TrackProfile()
        {
            CreateMap<Track, TrackDto>()
                .ForMember(dst => dst.TrackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Genres, opt => opt.MapFrom(src => src.Genres.Select(e => e.Name)))
                .ForMember(dst => dst.CoverUri, opt => opt.MapFrom(src => src.Release.ReleaseCoverUri))
                .ForMember(dst => dst.TrackParticipants, opt => opt.MapFrom(src => src.Participants));

            CreateMap<TrackParticipant, TrackParticipantDto>()
                .ForMember(dst => dst.Pseudonym, opt => opt.MapFrom(src => src.Artist.Pseudonym))
                .ForMember(dst => dst.ArtistId, opt => opt.MapFrom(src => src.Artist.Id))
                .ReverseMap();

            CreateMap<PagedList<Track>, PagedList<TrackDto>>()
               .ConvertUsing<PagedListConverter<Track, TrackDto>>();
        }
    }
}
