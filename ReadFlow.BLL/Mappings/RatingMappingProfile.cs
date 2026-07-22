using AutoMapper;
using ReadFlow.BLL.DTOs.Ratings;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class RatingMappingProfile : Profile
{
    public RatingMappingProfile()
    {
        CreateMap<BookRating, RatingDto>();

        CreateMap<CreateRatingDto, BookRating>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Book, opt => opt.Ignore());
    }
}
