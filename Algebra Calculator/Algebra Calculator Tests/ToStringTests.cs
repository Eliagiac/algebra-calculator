using System.Linq;
using static System.Math;
using static Utilities.Polynomials;

[TestClass]
public class ToStringTests
{
    // 2a
    [TestMethod]
    public void NumberToString_1Letter_v1_CorrectResult()
    {
        Number number = new(2, new() { new Letter('a') });

        Console.WriteLine(number.ToString());
        Assert.AreEqual("2a", number.ToString());
    }

    // 2a^2
    [TestMethod]
    public void NumberToString_1Letter_v2_CorrectResult()
    {
        Number number = new(2, new() { new Letter('a', new(2)) });

        Console.WriteLine(number.ToString());
        Assert.AreEqual("2a^2", number.ToString());
    }

    // 3a^(5x+1)
    [TestMethod]
    public void NumberToString_1Letter_v3_CorrectResult()
    {
        Number number = new(3, new() {
            new Letter('a', new(new List<Factor>() {
                new Expression(new List<Number>() {
                    new(5, new() { new Letter('x') }),
                    new(1)
                })
            }))
        });

        Console.WriteLine(number.ToString());
        Assert.AreEqual("3a^(5x+1)", number.ToString());
    }
}
