using System.Linq;
using static System.Math;

public abstract class Value
{
    public readonly double DecimalValue;

    protected Value(double decimalValue)
    {
        DecimalValue = decimalValue;
    }

    protected Value(Value other)
    {
        DecimalValue = other.DecimalValue;
    }

    public abstract Value Copy();

    public abstract override bool Equals(object? obj);

    public abstract override int GetHashCode();

    public static implicit operator Value(int value) => 
        new RationalValue(value);
}

public class RationalValue : Value
{
    public readonly int Numerator;
    public readonly int Denominator;

    public RationalValue(int numerator, int denominator) : base((double)numerator / denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public RationalValue(int numerator) : base((double)numerator)
    {
        Numerator = numerator;
        Denominator = 1;
    }


    public RationalValue(RationalValue other) : base(other)
    {
        Numerator = other.Numerator;
        Denominator = other.Denominator;
    }

    public override Value Copy() =>
        new RationalValue(this);

    // Note: may prefer to compare the DecimalValue of the values instead of the Numerator and Denominator,
    // so that, for example, 4/2 and 6/3 are considered equal. One concern is this could potentially
    // mark numbers as not equal because of rounding errors in the decimal value calculation.
    public override bool Equals(object? obj)
    {
        var value = obj as RationalValue;
        if (value == null) return false;

        return
            value.Numerator.Equals(Numerator) &&
            value.Denominator.Equals(Denominator);
    }

    public override int GetHashCode()
    {
        return 
            Numerator.GetHashCode() ^ 
            Denominator.GetHashCode();
    }


    public List<int> PrimeFactors()
    {
        List<int> factors = new() { 1 };

        // Factoring fractional numbers is not yet supported.
        if (Denominator != 1) return factors;

        int value = Numerator;

        // Trial division algorithm:
        // Try to divide the value by all numbers below the value's square root.
        // Only odd numbers are tried after 2, to avoid unnecessary divisions.
        int divisor = 2;
        while (value > 1 && divisor * divisor <= value)
        {
            if (value % divisor == 0)
            {
                factors.Add(divisor);

                value /= divisor;
                divisor += divisor == 2 ? 1 : 2;
            }
        }

        return factors;
    }
}

public class IrrationalValue : Value
{
    public readonly RationalValue Coefficient;
    public readonly Sign Sign;
    public readonly RationalValue Radical;

    /// <summary>
    /// The decimal value resulting from the expression Coefficient - √Radical, 
    /// used in case the sign used is ± (which gives 2 different results).
    /// </summary>
    public readonly double SecondDecimalValue;

    /// <summary>
    /// Create an irrational value with two different possible values.
    /// </summary>
    /// <remarks>An example is 3 ± √2.</remarks>
    public IrrationalValue(RationalValue coefficient, RationalValue radical) :
        base(coefficient.DecimalValue + Sqrt(radical.DecimalValue))
    {
        SecondDecimalValue = coefficient.DecimalValue - Sqrt(radical.DecimalValue);

        Coefficient = coefficient;
        Radical = radical;
    }

    /// <summary>
    /// Create an irrational value with the given sign for the radical part.
    /// </summary>
    /// <remarks>Some examples are 1 + √3 and 7 - √11.</remarks>
    public IrrationalValue(RationalValue coefficient, Sign sign, RationalValue radical) :
        base(coefficient.DecimalValue + (int)sign * Sqrt(radical.DecimalValue))
    {
        Coefficient = coefficient;
        Radical = radical;
    }

    public IrrationalValue(IrrationalValue other) : base(other)
    {
        Coefficient = other.Coefficient;
        Radical = other.Radical;
        SecondDecimalValue = other.SecondDecimalValue;
    }

    public override Value Copy() =>
        new IrrationalValue(this);

    public override bool Equals(object? obj)
    {
        var value = obj as IrrationalValue;
        if (value == null) return false;

        return
            value.Coefficient.Equals(Coefficient) &&
            value.Sign.Equals(Sign) &&
            value.Radical.Equals(Radical);
    }

    public override int GetHashCode()
    {
        return
            Coefficient.GetHashCode() ^
            Sign.GetHashCode() ^
            Radical.GetHashCode();
    }
}

public enum Operator
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}

public enum AdditiveOperator
{
    Addition = Operator.Addition,
    Subtraction = Operator.Subtraction,
}

public enum Sign
{
    Positive = 1,
    Negative = -1,
}