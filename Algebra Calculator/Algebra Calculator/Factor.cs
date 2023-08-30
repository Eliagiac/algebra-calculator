using System.Linq;
using static System.Math;

public abstract class Factor
{
    public Number Exponent;

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

    public abstract override string ToString();
}

public class Letter : Factor
{
    private readonly char _letter;

    public Letter(char letter) : base(new Number(1))
    {
        _letter = letter;
    }

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

    /// <remarks>Does not take exponents into account.</remarks>
    public override bool Equals(object? obj)
    {
        var factor = obj as Letter;
        if (factor == null) return false;

        return factor._letter.Equals(_letter);
    }

    /// <remarks>Does not take exponents into account.</remarks>
    public override int GetHashCode()
    {
        return _letter.GetHashCode();
    }

    public override string ToString()
    {
        if (Exponent.Equals(new Number(1))) return $"{_letter}";

        if (Exponent.Factors.Count == 0) return $"{_letter}^{Exponent}";

        else return $"{_letter}^({Exponent})";
    }
}

/// <summary>
/// An expression is a set of one or more terms linked by additive operators.
/// </summary>
public class Expression : Factor
{
    public readonly List<Number> Terms;

    public Expression(List<Number> terms) : base(new Number(1))
    {
        Terms = terms;
    }

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

    /// <remarks>Does not take exponents into account.</remarks>
    public override bool Equals(object? obj)
    {
        var factor = obj as Expression;
        if (factor == null) return false;

        return
            factor.Terms.SequenceEqual(Terms);
    }

    public override int GetHashCode()
    {
        return Terms.GetHashCode();
    }

    public override string ToString()
    {
        string terms = $"{string.Join('+', Terms)}";

        if (Exponent.Equals(new Number(1))) return $"{terms}";

        if (Exponent.Factors.Count == 0) return $"{terms}^{Exponent}";

        else return $"{terms}^({Exponent})";
    }
}
