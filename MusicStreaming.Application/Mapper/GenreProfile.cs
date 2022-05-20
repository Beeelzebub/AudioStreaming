using AutoMapper;
using MusicStreaming.Application.DTOs.Genres;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Application.Mapper
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreDto, Genre>();
            CreateMap<IEnumerable<Genre>, IEnumerable<GenreDto>>().ReverseMap();
        }
    }
}
