using AutoMapper;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mapper.Converters;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Application.Mapper
{
    public class ReleaseProfile : Profile
    {
        public ReleaseProfile()
        {
            CreateMap<CreateReleaseDto, Release>();

            CreateMap<Release, ReleaseDto>()
                .ForMember(dst => dst.Participants, opt => opt.MapFrom(src => src.Artists))
                .ForMember(dst => dst.ReleaseId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PagedList<Release>, PagedList<ReleaseDto>>()
                .ConvertUsing<PagedListConverter<Release, ReleaseDto>>();

            CreateMap<ReleaseParticipantDto, Artist>().ReverseMap();

            CreateMap<Release, ReleaseDetailDto>()
                .ForMember(dst => dst.ReleaseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Participants, opt => opt.MapFrom(src => src.Artists));

            CreateMap<PagedList<Release>, PagedList<ReleaseDetailDto>>()
                .ConvertUsing<PagedListConverter<Release, ReleaseDetailDto>>();
        }
    }
}
