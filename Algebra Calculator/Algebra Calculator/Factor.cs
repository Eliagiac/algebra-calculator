using System.Linq;
using System.Numerics;
using static System.Math;

public abstract class Factor
{
    public readonly Number Exponent;

    protected Factor(Number exponent)
    {
        Exponent = exponent;
    }

    protected Factor(Factor other)
    {
        Exponent = new(other.Exponent);
    }

    public abstract Factor Copy();

    public abstract override bool Equals(object? obj);

    public abstract override int GetHashCode();
}

public class Letter : Factor
{
    private readonly char _letter;

    public Letter(char letter, Number exponent) : base(exponent)
    {
        _letter = letter;
    }

    public Letter(Letter other) : base(other)
    {
        _letter = other._letter;
    }

    public override Factor Copy() =>
        new Letter(this);

    public override bool Equals(object? obj)
    {
        var factor = obj as Letter;
        if (factor == null) return false;

        return
            factor._letter.Equals(_letter) &&
            factor.Exponent.Equals(Exponent);
    }

    public override int GetHashCode()
    {
        return
            _letter.GetHashCode() ^ 
            Exponent.GetHashCode();
    }
}

/// <summary>
/// An expression is a set of one or more terms linked by additive operators.
/// </summary>
public class Expression : Factor
{
    public readonly List<Number> Terms;

    public Expression(List<Number> terms, Number exponent) : base(exponent)
    {
        Terms = terms;
    }

    public Expression(Expression other) : base(other)
    {
        Terms = other.Terms;
    }

    public override Factor Copy() =>
        new Expression(this);

    public override bool Equals(object? obj)
    {
        var factor = obj as Expression;
        if (factor == null) return false;

        return
            factor.Terms.SequenceEqual(Terms) &&
            factor.Exponent.Equals(Exponent);
    }

    public override int GetHashCode()
    {
        return Terms.GetHashCode();
    }
}