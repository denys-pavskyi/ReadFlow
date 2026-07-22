using AutoMapper;
using ReadFlow.BLL.DTOs.Posts;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.LikeCount, opt => opt.Ignore());

        CreateMap<CreatePostDto, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore());
    }
}
