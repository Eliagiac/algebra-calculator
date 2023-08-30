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

    public abstract override string ToString();


    public static implicit operator Value(int value) => 
        new RationalValue(value);

    /// <summary>
    /// Returns true if the two values have the same decimal value.
    /// </summary>
    /// <remarks>Unlike <see cref="RationalValue.Equals"/>, two values with different combinations of numerator 
    /// and denominator might be considered equal if the result of the division is the same, like 2/4 and 1/2.</remarks>
    public static bool operator ==(Value left, Value right)
    {
        if ((object)right == null) return (object)left == null;

        return Abs(left.DecimalValue - right.DecimalValue) < 1e-10;
    }

    public static bool operator !=(Value left, Value right) => !(left == right);

    /// <summary>
    /// Add <paramref name="left"/> and <paramref name="right"/>.
    /// </summary>
    public static Value operator +(Value left, Value right)
    {
        int leftNumerator = ((RationalValue)left).Numerator;
        int rightNumerator = ((RationalValue)right).Numerator;

        int leftDenominator = ((RationalValue)left).Denominator;
        int rightDenominator = ((RationalValue)right).Denominator;

        int commonDenominator = LCM(leftDenominator, rightDenominator);

        return new RationalValue(
            numerator: leftNumerator * (commonDenominator / leftDenominator) +
                       rightNumerator * (commonDenominator / rightDenominator),
            denominator: commonDenominator
        );
    }

    /// <summary>
    /// Subtract <paramref name="right"/> from <paramref name="left"/>.
    /// </summary>
    public static Value operator -(Value left, Value right) => left + -right;

    /// <summary>
    /// Negate the value of <paramref name="value"/>.
    /// </summary>
    /// <remarks>Negation can only be used on rational numbers with the current implementation.</remarks>
    public static Value operator -(Value value) => 
        new RationalValue(
            -((RationalValue)value).Numerator,
            ((RationalValue)value).Denominator
        );

    /// <summary>
    /// Divide <paramref name="left"/> by <paramref name="right"/>.
    /// </summary>
    /// <remarks>Can only be used with <see cref="RationalValue"/>s.</remarks>
    public static Value operator /(Value left, Value right) =>
        left * right.Reciprocal();

    /// <summary>
    /// Multiply <paramref name="left"/> by <paramref name="right"/>.
    /// </summary>
    /// <remarks>Can only be used with <see cref="RationalValue"/>s.</remarks>
    public static Value operator *(Value left, Value right) =>
        new RationalValue(
            numerator: ((RationalValue)left).Numerator * ((RationalValue)right).Numerator,
            denominator: ((RationalValue)left).Denominator * ((RationalValue)right).Denominator
        );


    /// <summary>
    /// Find the greatest common factor between the <paramref name="firstNumber"/> and the <paramref name="secondNumber"/> and return it.
    /// </summary>
    /// <remarks>The only numbers currently supported are integers (<see cref="RationalValue"/>s with a denominator of 1).</remarks>
    public static int GCF(Value firstNumber, Value secondNumber)
    {
        List<int> firstNumberPrimeFactors = ((RationalValue)firstNumber).PrimeFactors();
        List<int> secondNumberPrimeFactors = ((RationalValue)secondNumber).PrimeFactors();

        return firstNumberPrimeFactors
            .Where(secondNumberPrimeFactors.Remove)
            .Aggregate((x, y) => x * y);
    }

    /// <summary>
    /// Find the lowest common multiple between the <paramref name="firstNumber"/> and the <paramref name="secondNumber"/> and return it.
    /// </summary>
    /// <remarks>The only numbers currently supported are integers (<see cref="RationalValue"/>s with a denominator of 1).</remarks>
    public static int LCM(Value firstNumber, Value secondNumber)
    {
        List<int> firstNumberPrimeFactors = ((RationalValue)firstNumber).PrimeFactors();
        List<int> secondNumberPrimeFactors = ((RationalValue)secondNumber).PrimeFactors();

        // Take all factors from the first number.
        List<int> factors = new(firstNumberPrimeFactors);

        // Take any new factors from the second number.
        factors.AddRange(
            secondNumberPrimeFactors
            .Where(f => !firstNumberPrimeFactors.Remove(f)).ToList()
        );

        return factors.Aggregate((x, y) => x * y);
    }

    /// <summary>
    /// Return 1/<see cref="Value"/>.
    /// </summary>
    /// <remarks>Not yet implemented for values that aren't <see cref="RationalValue"/>s.</remarks>
    public virtual Value Reciprocal() => throw new NotImplementedException("The reciprocal of non-rational values was not yet implemented");
}

public class RationalValue : Value
{
    public readonly int Numerator;
    public readonly int Denominator;

    public RationalValue(int numerator, int denominator) : base((double)numerator / denominator)
    {
        int gcf = GCF(numerator, denominator);
        if (gcf != 1)
        {
            numerator /= gcf;
            denominator /= gcf;
        }

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
        if ((object?)value == null) return false;

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

    public override string ToString()
    {
        if (Denominator == 1) return Numerator.ToString();
        else return $"({Numerator}/{Denominator})";
    }


    public List<int> PrimeFactors()
    {
        List<int> factors = new() { 1 };

        // Factoring fractional numbers is not yet supported.
        if (Denominator != 1) return factors;

        int value = Numerator;

        // Trial division algorithm:
        // Try to divide the value by all numbers below the value's square root (and by itself).
        // Only odd numbers are tried after 2, to avoid unnecessary divisions.
        int divisor = 2;
        while (value > 1)
        {
            while (value % divisor == 0)
            {
                factors.Add(divisor);
                value /= divisor;
            }

            divisor += divisor == 2 ? 1 : 2;

            // If the divisor surpasses the root of the value, skip straight to the value itself.
            if (divisor * divisor > value) divisor = value;
        }

        return factors;
    }

    public override Value Reciprocal() 
    {
        return new RationalValue(Denominator, Numerator);
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
        if ((object?)value == null) return false;

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

    public override string ToString()
    {
        return $"{Coefficient}" +
            $"{(SecondDecimalValue != 0 ? '±' : Sign == Sign.Positive ? '+' : '-')}"+
            $"√{Radical}";
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
