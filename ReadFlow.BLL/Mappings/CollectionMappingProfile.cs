using AutoMapper;
using ReadFlow.BLL.DTOs.Collections;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class CollectionMappingProfile : Profile
{
    public CollectionMappingProfile()
    {
        CreateMap<Collection, CollectionDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.BookCount, opt => opt.Ignore());

        CreateMap<CreateCollectionDto, Collection>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.CollectionBooks, opt => opt.Ignore());
    }
}
