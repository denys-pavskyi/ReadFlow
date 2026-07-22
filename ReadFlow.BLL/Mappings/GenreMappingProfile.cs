using AutoMapper;
using ReadFlow.BLL.DTOs.Genres;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class GenreMappingProfile : Profile
{
    public GenreMappingProfile()
    {
        CreateMap<Genre, GenreDto>();
    }
}
