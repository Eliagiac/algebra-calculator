using System.Linq;
using static System.Math;
using static Utilities.Polynomials;

[TestClass]
public class PolynomialTests
{
    // 2a + 2b
    [TestMethod]
    public void FactorOutCommonFactor_2Terms_v1_CorrectResult()
    {
        Expression expression = new Expression(new List<Number>() {
            new(2, new() { new Letter('a') }),
            new(2, new() { new Letter('b') }) });

        HasCommonFactor(expression, out Number commonFactor);

        Assert.IsTrue(FactorOutCommonFactor(expression, commonFactor).Equals(
            new Number(2, new() {
                new Expression(new List<Number>() {
                    new(new List<Factor>() { new Letter('a') }),
                    new(new List<Factor>() { new Letter('b') })
                })
            })
        ));
    }
}
