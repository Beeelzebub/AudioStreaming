using AutoMapper;
using MusicStreaming.Application.DTOs.Releases;
using MusicStreaming.Application.DTOs.Responses;
using MusicStreaming.Application.Mapper.Converters;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Application.Mapper
{
    public class ReleaseProfile : Profile
    {
        public ReleaseProfile()
        {
            CreateMap<CreateReleaseDto, Release>();
            //.ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description)
            //.ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title)
            //.ForMember(dst => dst., opt => opt.MapFrom(src => src.Description)
            //.ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description)

            CreateMap<Release, ReleaseDto>()
                .ForMember(dst => dst.Participants, opt => opt.MapFrom(src => src.Artists));

            CreateMap<PagedList<Release>, PagedList<ReleaseDto>>()
                .ConvertUsing<PagedListConverter<Release, ReleaseDto>>();


        }
    }
}
