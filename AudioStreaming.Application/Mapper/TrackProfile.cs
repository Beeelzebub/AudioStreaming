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
                .ForMember(dst => dst.CoverUri, opt => opt.MapFrom(src => src.Release.ReleaseCoverUri));

            CreateMap<TrackParticipant, TrackParticipantDto>()
                .ForMember(dst => dst.Pseudonym, opt => opt.MapFrom(src => src.Artist.Pseudonym));

            CreateMap<PagedList<Track>, PagedList<TrackDto>>()
               .ConvertUsing<PagedListConverter<Track, TrackDto>>();
        }
    }
}
