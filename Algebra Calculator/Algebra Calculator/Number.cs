using System.Linq;
using static System.Math;

public class Number
{
    /// <summary>
    /// The numeric part of the monomial.
    /// </summary>
    public readonly Value Coefficient;

    /// <summary>
    /// The literal part of the monomial, as well as any other factors.
    /// </summary>
    public readonly List<Factor> Factors;

    /// <summary>
    /// Create a new number with a copy of the given coefficient.
    /// </summary>
    public Number(Value coefficient)
    {
        Coefficient = coefficient.Copy();
        Factors = new();
    }

    /// <summary>
    /// Create a new number with a copy of the given coefficient and a copy of the given factors.
    /// </summary>
    public Number(Value coefficient, List<Factor> factors)
    {
        Coefficient = coefficient.Copy();
        Factors = factors.Select(f => f.Copy()).ToList();
    }

    public Number(Number other)
    {
        Coefficient = other.Coefficient.Copy();
        Factors = other.Factors.Select(f => f.Copy()).ToList();
    }

    /// <summary>
    /// The number is a constant / pure number.
    /// </summary>
    /// <remarks>Some examples include 2, 3, -6, 1/2 and √2.</remarks>
    public bool IsConstant => Factors.Count == 0;


    public override bool Equals(object? obj)
    {
        var number = obj as Number;
        if (number == null) return false;

        return
            number.Coefficient.Equals(Coefficient) &&
            number.Factors.SequenceEqual(Factors);
    }

    public override int GetHashCode()
    {
        return
            Coefficient.GetHashCode() ^
            Factors.GetHashCode();
    }
}

//public class Fraction : Number
//{
//
//}
