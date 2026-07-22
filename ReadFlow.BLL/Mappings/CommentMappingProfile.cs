using AutoMapper;
using ReadFlow.BLL.DTOs.Comments;
using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Mappings;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Score, opt => opt.Ignore())
            .ForMember(dest => dest.Replies, opt => opt.Ignore());

        CreateMap<CreateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Book, opt => opt.Ignore())
            .ForMember(dest => dest.ParentComment, opt => opt.Ignore())
            .ForMember(dest => dest.Replies, opt => opt.Ignore())
            .ForMember(dest => dest.Votes, opt => opt.Ignore());
    }
}
