namespace ReadFlow.BLL.ValueObjects;

public readonly struct VoteScore : IEquatable<VoteScore>
{
    private readonly int _score;

    public int Score => _score;

    private VoteScore(int score)
    {
        _score = score;
    }

    public static VoteScore FromVotes(int upvotes, int downvotes)
    {
        return new VoteScore(upvotes - downvotes);
    }

    public static VoteScore Zero => new VoteScore(0);

    public static VoteScore operator ++(VoteScore score) => new VoteScore(score._score + 1);
    public static VoteScore operator --(VoteScore score) => new VoteScore(score._score - 1);

    public static bool operator ==(VoteScore left, VoteScore right) => left._score == right._score;
    public static bool operator !=(VoteScore left, VoteScore right) => left._score != right._score;

    public static implicit operator int(VoteScore score) => score._score;

    public bool Equals(VoteScore other) => _score == other._score;
    public override bool Equals(object? obj) => obj is VoteScore other && Equals(other);
    public override int GetHashCode() => _score.GetHashCode();

    public override string ToString() => _score >= 0 ? $"+{_score}" : _score.ToString();
}
