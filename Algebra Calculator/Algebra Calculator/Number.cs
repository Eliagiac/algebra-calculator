using System.Linq;
using static System.Math;

public class Number
{
    /// <summary>
    /// The numeric part of the monomial.
    /// </summary>
    public Value Coefficient;

    /// <summary>
    /// The literal part of the monomial, as well as any other factors.
    /// </summary>
    public List<Factor> Factors;

    /// <summary>
    /// Create a new number with a copy of the given coefficient.
    /// </summary>
    public Number(Value coefficient)
    {
        Coefficient = coefficient.Copy();
        Factors = new();
    }

    /// <summary>
    /// Create a new number with a copy of the given factors.
    /// </summary>
    public Number(List<Factor> factors)
    {
        Coefficient = (Value)1;
        Factors = factors.Select(f => f.Copy()).ToList();
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


    /// <summary>
    /// Add <paramref name="left"/> and <paramref name="right"/>.
    /// </summary>
    /// <remarks>Expects numbers with the same factors.</remarks>
    public static Number operator +(Number left, Number right)
    {
        return new Number(
            coefficient: left.Coefficient + right.Coefficient,
            factors: left.Factors
        );
    }

    /// <summary>
    /// Subtract <paramref name="right"/> from <paramref name="left"/>.
    /// </summary>
    /// <remarks>Expects numbers with the same factors.</remarks>
    public static Number operator -(Number left, Number right)
    {
        return new Number(
            coefficient: left.Coefficient - right.Coefficient,
            factors: left.Factors
        );
    }

    /// <summary>
    /// Negate <paramref name="number"/>.
    /// </summary>
    /// <remarks>Expects numbers with the same factors.</remarks>
    public static Number operator -(Number number)
    {
        return new Number(
            coefficient: -number.Coefficient,
            factors: number.Factors
        );
    }

    /// <summary>
    /// Divide <paramref name="left"/> by <paramref name="right"/>.
    /// </summary>
    /// <remarks>Only whole number, numerical exponents are currently supported.</remarks>
    public static Number operator /(Number left, Number right)
    {
        // Only numerical exponents are currently supported for division.
        if (!left.Factors.All(f => f.Exponent.Factors.Count == 0))
            throw new NotImplementedException("Only division between numbers with factors with numerical exponents is currently supported.");

        // Exponents must all be integers.
        if (!left.Factors.All(f => ((RationalValue)f.Exponent.Coefficient).Denominator == 1))
            throw new NotImplementedException("Only division between numbers with factors with whole number exponents is currently supported");

        // Having a deep copy of the divisor is essential because all factors will be removed from it.
        Number divisor = new(right);

        Value coefficient = left.Coefficient / divisor.Coefficient;

        List<Factor> factors = new();
        for (int i = 0; i < left.Factors.Count; i++)
        {
            Factor result = left.Factors[i].Copy();

            for (int j = 0; j < divisor.Factors.Count; j++)
            {
                // The Equals method does not consider exponents.
                if (divisor.Factors[j].Equals(result))
                {
                    // Remove the factor's exponent from the result.
                    // May result in a negative exponent.
                    result.Exponent -= (divisor.Factors[j]).Exponent;
                    
                    // If the resulting exponent is negative, the dividing
                    // factor can still be used on other factors of the result.
                    (divisor.Factors[j]).Exponent = new((int)Max(-result.Exponent.Coefficient.DecimalValue, 0));
                    result.Exponent = new((int)Max(result.Exponent.Coefficient.DecimalValue, 0));
                }

                // Remove factors if their exponent reaches 0.
                if (divisor.Factors[j].Exponent.Coefficient == (Value)0)
                    divisor.Factors.RemoveAt(j);
            }

            if (result.Exponent.Coefficient != (Value)0)
                factors.Add(result);
        }

        return new(coefficient, factors);
    }


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

    public override string ToString()
    {
        string factors = $"{string.Join("", Factors.Select(f => f.ToString()))}";

        if (Coefficient == (Value)1 && Factors.Count > 0) return $"{factors}";
        else return $"{Coefficient}{factors}";
    }
}

//public class Fraction : Number
//{
//
//}
