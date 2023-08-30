using System.Linq;
using static System.Math;

namespace Utilities 
{
    /// <summary>
    /// The <see cref="Polynomials"/> class contains static methods that can be used on <see cref="Expression"/>s that classify as polynomials.
    /// </summary> 
    public class Polynomials
    {
        /// <summary>
        /// Collect together repeated factors in a list of factors.
        /// </summary>
        /// <remarks>Exponents of similar factors have to be similar to each other.</remarks>
        public static void SimplifyFactors(ref List<Factor> factors)
        {
            for (int i = 0; i < factors.Count; i++)
            {
                Number exponent = new(factors[i].Exponent);

                for (int j = i + 1; j < factors.Count; j++)
                {
                    if (factors[j].Equals(factors[i]))
                    {
                        exponent += factors[j].Exponent;
                        factors.RemoveAt(j--);
                    }
                }

                factors[i].Exponent = exponent;
            }
        }

        public static Number? Factorise(Expression polynomial)
        {
            Number result = null;

            foreach (Number term in polynomial.Terms) SimplifyFactors(ref term.Factors);

            // Collect any common factors from the terms of the polynomial.
            if (HasCommonFactor(polynomial, out Number commonFactor)) result = FactorOutCommonFactor(polynomial, commonFactor);

            return null;
        }

        public static bool HasCommonFactor(Expression polynomial, out Number commonFactor)
        {
            commonFactor = new(polynomial.Terms[0]);

            for (int i = 1; i < polynomial.Terms.Count; i++)
            {
                // It's essential that currentTerm is a deep copy of the current terms (some factors will be removed).
                Number currentTerm = new(polynomial.Terms[i]);

                commonFactor = new(
                    coefficient: Value.GCF(
                        commonFactor.Coefficient, 
                        currentTerm.Coefficient
                    ),
                    factors: CommonFactors(commonFactor.Factors, currentTerm.Factors)
                );
            }

            return commonFactor.Factors.Count > 0 || commonFactor.Coefficient != (Value)1;
        }

        /// <remarks>Only whole number, numerical exponents are currently supported. <br />
        /// All equal factors must be grouped together for this function to work (see <see cref="SimplifyFactors"/>.</remarks>
        public static List<Factor> CommonFactors(List<Factor> left, List<Factor> right)
        {
            List<Factor> factors = new();
            for (int i = 0; i < left.Count; i++)
            {
                // Copy only the literal/expression part of the factor without the exponent.
                Factor result = left[i].Copy();
                result.Exponent = new(0);

                for (int j = 0; j < right.Count; j++)
                {
                    // The Equals method does not consider exponents.
                    if (right[j].Equals(result))
                    {
                        // Choose the lowest exponent from the two factors.
                        result.Exponent = new((int)Min(
                            left[i].Exponent.Coefficient.DecimalValue,
                            right[j].Exponent.Coefficient.DecimalValue));
                    }
                }
            
                if (result.Exponent.Coefficient != (Value)0)
                    factors.Add(result);
            }

            return factors;
        }

        public static Number FactorOutCommonFactor(Expression polynomial, Number commonFactor)
        {
            Number result = new(commonFactor);
            result.Factors.Add(new Expression(polynomial.Terms.Select(t => t / commonFactor).ToList()));
            return result;
        }
    }
}
