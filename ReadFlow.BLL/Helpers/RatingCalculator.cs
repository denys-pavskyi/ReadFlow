using ReadFlow.DAL.Entities;

namespace ReadFlow.BLL.Helpers;

public static class RatingCalculator
{
    public static decimal CalculateAverageRating(IEnumerable<BookRating> ratings)
    {
        if (!ratings.Any())
            return 0m;

        return Math.Round((decimal)ratings.Average(r => r.Rating), 2);
    }

    public static int CalculateCommentScore(IEnumerable<CommentVote> votes)
    {
        if (!votes.Any())
            return 0;

        var upvotes = votes.Count(v => v.IsUpvote);
        var downvotes = votes.Count(v => !v.IsUpvote);

        return upvotes - downvotes;
    }
}
