using AutoMapper;
using ReadFlow.BLL.DTOs.Books;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
            .ForMember(dest => dest.RatingCount, opt => opt.Ignore());

        CreateMap<Book, BookDetailDto>()
            .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
            .ForMember(dest => dest.RatingCount, opt => opt.Ignore())
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BookGenres.Select(bg => bg.Genre)));

        CreateMap<CreateBookDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.BookGenres, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.CollectionBooks, opt => opt.Ignore());

        CreateMap<UpdateBookDto, Book>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
