using AutoMapper;
using MusicStreaming.Application.DTOs.Release;
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
        }
    }
}
