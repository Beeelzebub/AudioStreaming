using AutoMapper;
using AudioStreaming.Application.DTOs.Genres;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Application.Mapper
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreDto, Genre>();

            CreateMap<ICollection<Genre>, ICollection<GenreDto>>().ReverseMap();
        }
    }
}
