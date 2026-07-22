using AutoMapper;
using ReadFlow.BLL.DTOs.Reviews;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

        CreateMap<CreateReviewDto, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Book, opt => opt.Ignore());
    }
}
