using System.Linq;
using static System.Math;
using static Utilities.Polynomials;

[TestClass]
public class ValueTests
{
    [TestMethod]
    public void PrimeFactors_2_CorrectResult()
    {
        Assert.IsTrue(((RationalValue)2).PrimeFactors().SequenceEqual(new List<int>() { 1, 2 }));
    }

    [TestMethod]
    public void PrimeFactors_4_CorrectResult()
    {
        Assert.IsTrue(((RationalValue)4).PrimeFactors().SequenceEqual(new List<int>() { 1, 2, 2 }));
    }

    [TestMethod]
    public void PrimeFactors_7_CorrectResult()
    {
        Assert.IsTrue(((RationalValue)7).PrimeFactors().SequenceEqual(new List<int>() { 1, 7 }));
    }

    [TestMethod]
    public void PrimeFactors_22_CorrectResult()
    {
        Assert.IsTrue(((RationalValue)22).PrimeFactors().SequenceEqual(new List<int>() { 1, 2, 11 }));
    }

    [TestMethod]
    public void PrimeFactors_84_CorrectResult()
    {
        Assert.IsTrue(((RationalValue)84).PrimeFactors().SequenceEqual(new List<int>() { 1, 2, 2, 3, 7 }));
    }

    [TestMethod]
    public void PrimeFactors_9625_CorrectResult()
    {
        Assert.IsTrue(((RationalValue)9625).PrimeFactors().SequenceEqual(new List<int>() { 1, 5, 5, 5, 7, 11 }));
    }


    [TestMethod]
    public void GCF_2And4_Result2()
    {
        Assert.IsTrue(Value.GCF(2, 4) == 2);
    }

    [TestMethod]
    public void GCF_4And7_Result2()
    {
        Assert.IsTrue(Value.GCF(4, 7) == 1);
    }

    [TestMethod]
    public void GCF_8And12_Result2()
    {
        Assert.IsTrue(Value.GCF(8, 12) == 4);
    }

    [TestMethod]
    public void GCF_252And420_Result84()
    {
        Assert.IsTrue(Value.GCF(252, 420) == 84);
    }


    [TestMethod]
    public void LCM_2And4_Result4()
    {
        Assert.IsTrue(Value.LCM(2, 4) == 4);
    }

    [TestMethod]
    public void LCM_4And7_Result28()
    {
        Assert.IsTrue(Value.LCM(4, 7) == 28);
    }

    [TestMethod]
    public void LCM_8And12_Result24()
    {
        Assert.IsTrue(Value.LCM(8, 12) == 24);
    }

    [TestMethod]
    public void LCM_75And580_Result8700()
    {
        Assert.IsTrue(Value.LCM(75, 580) == 8700);
    }


    [TestMethod]
    public void ValueAdd_2And2_CorrectResult()
    {
        Value sum = (Value)2 + (Value)2;

        Assert.AreEqual(sum, (Value)4);
    }

    [TestMethod]
    public void ValueAdd_3And9_CorrectResult()
    {
        Value sum = (Value)3 + (Value)9;

        Assert.AreEqual(sum, (Value)12);
    }

    [TestMethod]
    public void ValueAdd_IntegerAndFraction_CorrectResult()
    {
        Value sum = (Value)3 + new RationalValue(1, 2);

        Assert.AreEqual(sum, new RationalValue(7, 2));
    }

    [TestMethod]
    public void ValueAdd_FractionAndFraction_CorrectResult()
    {
        Value sum = new RationalValue(3, 5) + new RationalValue(6, 7);

        Assert.AreEqual(sum, new RationalValue(51, 35));
    }
}

[TestClass]
public class NumberTests
{
    // 2 + 2 = 4
    [TestMethod]
    public void NumberAdd_ValidIntegers_CorrectResult()
    {
        Number sum =
            new Number(
                coefficient: 2
            ) +
            new Number(
                coefficient: 2
            );

        Assert.AreEqual(sum, new Number(4));
    }

    // 3a + 2a = 5a
    [TestMethod]
    public void NumberAdd_ValidMonomials_CorrectResult()
    {
        Number sum =
            new Number(
                coefficient: 3,
                factors: new() { new Letter('a') }
            ) +
            new Number(
                coefficient: 2,
                factors: new() { new Letter('a') }
            );

        Assert.AreEqual(sum, new Number(5, new() { new Letter('a') }));
    }
}

[TestClass]
public class FactorTests
{
    [TestMethod]
    public void CommonFactors_WithoutCommonFactors_CorrectResult()
    {
        List<Factor> commonFactors = CommonFactors(
            new List<Factor>() { new Letter('a') },
            new List<Factor>() { new Letter('b') }
        );

        Assert.IsTrue(commonFactors.SequenceEqual(new List<Factor>() { }));
    }

    [TestMethod]
    public void CommonFactors_WithCommonFactors_v1_CorrectResult()
    {
        List<Factor> commonFactors = CommonFactors(
            new List<Factor>() { new Letter('a') },
            new List<Factor>() { new Letter('a') }
        );

        Assert.IsTrue(commonFactors.SequenceEqual(new List<Factor>() { new Letter('a') }));
    }

    [TestMethod]
    public void CommonFactors_WithCommonFactors_v2_CorrectResult()
    {
        List<Factor> commonFactors = CommonFactors(
            new List<Factor>() { new Letter('a', new(2)) },
            new List<Factor>() { new Letter('a'), new Letter('b') }
        );

        Assert.IsTrue(commonFactors.SequenceEqual(new List<Factor>() { new Letter('a') }));
    }

    [TestMethod]
    public void CommonFactors_WithCommonFactors_v3_CorrectResult()
    {
        List<Factor> commonFactors = CommonFactors(
            new List<Factor>() { new Letter('a', new(3)), new Letter('b', new(2)) },
            new List<Factor>() { new Letter('a'), new Letter('b'), new Letter('c', new(2)) }
        );

        Assert.IsTrue(commonFactors.SequenceEqual(new List<Factor>() { new Letter('a'), new Letter('b') }));
    }


    // 2 + 4 (GCF: 2)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v1_FactorSeen()
    {
        Assert.IsTrue(
            HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(2),
                    new(4)
                    }
                ),
                out Number commonFactor));
    }

    // 2 + 4 (GCF: 2)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v1_CorrectFactor()
    {
        HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(2),
                    new(4)
                    }
                ),
                out Number commonFactor);

        Assert.AreEqual(commonFactor, new(2));
    }

    // 2x + 4 (GCF: 2)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v2_FactorSeen()
    {
        Assert.IsTrue(
            HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(2, new() { new Letter('x') }),
                    new(4)
                    }
                ),
                out Number commonFactor));
    }

    // 2x + 4 (GCF: 2)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v2_CorrectFactor()
    {
        HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(2, new() { new Letter('x') }),
                    new(4)
                    }
                ),
                out Number commonFactor);

        Assert.AreEqual(commonFactor, new(2));
    }

    // 2x + 4x (GCF: 2x)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v3_FactorSeen()
    {
        Assert.IsTrue(
            HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(2, new() { new Letter('x') }),
                    new(4, new() { new Letter('x') })
                    }
                ),
                out Number commonFactor));
    }

    // 2x + 4x (GCF: 2x)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v3_CorrectFactor()
    {
        HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(2, new() { new Letter('x') }),
                    new(4, new() { new Letter('x') })
                    }
                ),
                out Number commonFactor);

        Assert.AreEqual(commonFactor, new(2, new() { new Letter('x') }));
    }

    // 8x^2yz + 12xy^3 (GCF: 4xy)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v4_FactorSeen()
    {
        Assert.IsTrue(
            HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(8, new() { new Letter('x', new(2)), new Letter('y'), new Letter('z') }),
                    new(12, new() { new Letter('x'), new Letter('y', new(3)) })
                    }
                ),
                out Number commonFactor));
    }

    // 8x^2yz + 12xy^3 (GCF: 4xy)
    [TestMethod]
    public void HasCommonFactor_WithCommonFactor_v4_CorrectFactor()
    {
        HasCommonFactor(
                new Expression(
                    new List<Number>() {
                    new(8, new() { new Letter('x', new(2)), new Letter('y'), new Letter('z') }),
                    new(12, new() { new Letter('x'), new Letter('y', new(3)) })
                    }
                ),
                out Number commonFactor);

        Assert.AreEqual(commonFactor, new(4, new() { new Letter('x'), new Letter('y') }));
    }


    [TestMethod]
    public void SimplifyFactors_1Factor_CorrectResult()
    {
        List<Factor> factors = new() { new Letter('a', new(2)) };

        SimplifyFactors(ref factors);

        Assert.IsTrue(
            factors.SequenceEqual(new List<Factor> { new Letter('a', new(2)) })
        );
    }

    [TestMethod]
    public void SimplifyFactors_2Factors_v1_CorrectResult()
    {
        List<Factor> factors = new() { new Letter('a', new(2)), new Letter('b') };

        SimplifyFactors(ref factors);

        Assert.IsTrue(
            factors.SequenceEqual(new List<Factor> { new Letter('a', new(2)), new Letter('b') })
        );
    }

    [TestMethod]
    public void SimplifyFactors_2Factors_v2_CorrectResult()
    {
        List<Factor> factors = new() { new Letter('a'), new Letter('a') };

        SimplifyFactors(ref factors);

        Assert.IsTrue(
            factors.SequenceEqual(new List<Factor> { new Letter('a', new(2)) })
        );
    }

    [TestMethod]
    public void SimplifyFactors_2Factors_v3_CorrectResult()
    {
        List<Factor> factors = new() { new Letter('a', new(2)), new Letter('a', new(3)) };

        SimplifyFactors(ref factors);

        Assert.IsTrue(
            factors.SequenceEqual(new List<Factor> { new Letter('a', new(5)) })
        );
    }

    [TestMethod]
    public void SimplifyFactors_3Factors_v1_CorrectResult()
    {
        List<Factor> factors = new() { new Letter('a'), new Letter('b'), new Letter('c') };

        SimplifyFactors(ref factors);

        Assert.IsTrue(
            factors.SequenceEqual(new List<Factor> { new Letter('a'), new Letter('b'), new Letter('c') })
        );
    }

    [TestMethod]
    public void SimplifyFactors_3Factors_v2_CorrectResult()
    {
        List<Factor> factors = new() { new Letter('a', new(2)), new Letter('b'), new Letter('a') };

        SimplifyFactors(ref factors);

        Assert.IsTrue(
            factors.SequenceEqual(new List<Factor> { new Letter('a', new(3)), new Letter('b') })
        );
    }
}
