#define TESTSTRING

using System.Text;
[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TestInputUtil;

using InputUtil.Current;

using System.Globalization;

public class InputUtilUnitTest
{
    private static void RedirectStdIn(string input)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(input + Environment.NewLine));
        Console.SetIn(new StreamReader(stream));
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("42", 42)]
    [InlineData("-1", -1)]
    [InlineData("2147483647", 2147483647)]
    [InlineData("-2147483648", -2147483648)]
    public void TestOneIntInput(string input, int expected)
    {
        RedirectStdIn(input);
        int result = InputUtil.ReadLine<int>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0.1", 0.1)]
    [InlineData("3.14", 3.14)]
    public void TestOneDoubleInput(string input, double expected)
    {
        RedirectStdIn(input);
        double result = InputUtil.ReadLine<double>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("9223372036854775807", 9223372036854775807)]
    [InlineData("-9223372036854775808", -9223372036854775808)]
    public void TestOnelongInput(string input, double expected)
    {
        RedirectStdIn(input);
        double result = InputUtil.ReadLine<double>();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TestOneDecimalInput()
    {
        RedirectStdIn("0.1");
        decimal result = InputUtil.ReadLine<decimal>();
        Assert.Equal(0.1m, result);
    }

#if TESTSTRING
    [Theory]
    [InlineData("abc")]
    [InlineData("‚ ‚¢‚¤‚¦‚¨")]
    public void TestOneStringInput(string input)
    {
        RedirectStdIn(input);
        string result = InputUtil.ReadLineString();
        Assert.Equal(input, result);
    }
#endif

    [Theory]
    [InlineData("1 2", new[] { 1, 2 })]
    [InlineData("1 100", new[] { 1, 100 })]
    [InlineData("-2147483648 0", new[] { -2147483648, 0 })]
    public void TestTwoIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
    }

    // this is not required behavior, but it is desirable
    [Fact]
    public void TestTwoInputTrailingSpace()
    {
        RedirectStdIn("1 2 ");
        var (a, b) = InputUtil.ReadLine<int>();
        Assert.Equal(1, a);
        Assert.Equal(2, b);
    }

    // this is not required behavior, but it is desirable
    [Fact]
    public void TestTwoInputExtraData()
    {
        RedirectStdIn("1 2 3");
        var (a, b) = InputUtil.ReadLine<int>();
        Assert.Equal(1, a);
        Assert.Equal(2, b);
    }

    [Theory]
    [InlineData("1 2 3", new[] { 1, 2, 3 })]
    [InlineData("1 100 1000", new[] { 1, 100, 1000 })]
    [InlineData("-2147483648 0 2147483647", new[] { -2147483648, 0, 2147483647 })]
    public void TestThreeIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
    }

    [Theory]
    [InlineData("1 2 3 4", new[] { 1, 2, 3, 4 })]
    [InlineData("1 10 100 1000", new[] { 1, 10, 100, 1000 })]
    [InlineData("-2147483648 0 2147483647 123456789", new[] { -2147483648, 0, 2147483647, 123456789 })]
    public void TestFourIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c, d) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
        Assert.Equal(expected[3], d);
    }

    [Theory]
    [InlineData("1 2 3 4 5", new[] { 1, 2, 3, 4, 5 })]
    [InlineData("1 10 100 1000 10000", new[] { 1, 10, 100, 1000, 10000 })]
    [InlineData("-2147483648 0 2147483647 123456789 987654321", new[] { -2147483648, 0, 2147483647, 123456789, 987654321 })]
    public void TestFiveIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c, d, e) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
        Assert.Equal(expected[3], d);
        Assert.Equal(expected[4], e);
    }

    [Theory]
    [InlineData("1 2 3 4 5 6", new[] { 1, 2, 3, 4, 5, 6 })]
    [InlineData("1 10 100 1000 10000 100000", new[] { 1, 10, 100, 1000, 10000, 100000 })]
    [InlineData("-2147483648 0 2147483647 123456789 987654321 567890123", new[] { -2147483648, 0, 2147483647, 123456789, 987654321, 567890123 })]
    public void TestSixIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c, d, e, f) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
        Assert.Equal(expected[3], d);
        Assert.Equal(expected[4], e);
        Assert.Equal(expected[5], f);
    }

    [Theory]
    [InlineData("1 2 3 4 5 6 7", new[] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData("1 10 100 1000 10000 100000 1000000", new[] { 1, 10, 100, 1000, 10000, 100000, 1000000 })]
    [InlineData("-2147483648 0 2147483647 123456789 987654321 567890123 1357924680", new[] { -2147483648, 0, 2147483647, 123456789, 987654321, 567890123, 1357924680 })]
    public void TestSevenIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c, d, e, f, g) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
        Assert.Equal(expected[3], d);
        Assert.Equal(expected[4], e);
        Assert.Equal(expected[5], f);
        Assert.Equal(expected[6], g);
    }

    [Theory]
    [InlineData("1 2 3 4 5 6 7 8", new[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
    [InlineData("1 10 100 1000 10000 100000 1000000 10000000", new[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 })]
    [InlineData("-2147483648 0 2147483647 123456789 987654321 567890123 1357924680 876543210", new[] { -2147483648, 0, 2147483647, 123456789, 987654321, 567890123, 1357924680, 876543210 })]
    public void TestEightIntInput(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c, d, e, f, g, h) = InputUtil.ReadLine<int>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
        Assert.Equal(expected[3], d);
        Assert.Equal(expected[4], e);
        Assert.Equal(expected[5], f);
        Assert.Equal(expected[6], g);
        Assert.Equal(expected[7], h);
    }

    [Fact]
    public void TestTwoDecimalInput()
    {
        RedirectStdIn("0.1 0.2");
        var (a, b) = InputUtil.ReadLine<decimal>();
        Assert.Equal(0.1m, a);
        Assert.Equal(0.2m, b);
        Assert.Equal(0.3m, a + b);
    }

    // cannot use decimal in InlineData
    [Theory]
    [InlineData("0.1", "0.2", "0.3")]
    [InlineData("1.23", "3.21", "4.44")]
    [InlineData("-10.99", "10.99", "0.0")]
    public void TestTwoDecimalInputString(string input1, string input2, string expectedSum)
    {
        string input = $"{input1} {input2}";
        RedirectStdIn(input);
        var (a, b) = InputUtil.ReadLine<decimal>();
        Assert.Equal(decimal.Parse(input1), a);
        Assert.Equal(decimal.Parse(input2), b);
        Assert.Equal(decimal.Parse(expectedSum), a + b);
    }

    [Theory]
    [InlineData("0.1 0.2", new[] { 0.1, 0.2 })]
    [InlineData("1.23 3.21", new[] { 1.23, 3.21 })]
    [InlineData("-10.99 10.99", new[] { -10.99, 10.99 })]
    public void TestTwoDoubleInput(string input, double[] expected)
    {
        RedirectStdIn(input);
        var (a, b) = InputUtil.ReadLine<double>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
    }

    [Theory]
    [InlineData("a b", new[] { 'a', 'b' })]
    [InlineData("# &", new[] { '#', '&' })]
    [InlineData("‚  ‚¢", new[] { '‚ ', '‚¢' })]
    public void TestTwoCharInput(string input, char[] expected)
    {
        RedirectStdIn(input);
        var (a, b) = InputUtil.ReadLine<char>();
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
    }

    [Fact]
    public void TestNotEnoughInput()
    {
        RedirectStdIn("1");
        Assert.Throws<FormatException>(() =>
        {
            var (a, b) = InputUtil.ReadLine<int>();
        });
    }

    [Fact]
    public void TestInvalidTypeOne()
    {
        RedirectStdIn("a");
        Assert.Throws<FormatException>(() =>
        {
            int a = InputUtil.ReadLine<int>();
        });
    }

    [Fact]
    public void TestInvalidTypeTwo()
    {
        RedirectStdIn("a b");
        Assert.Throws<FormatException>(() =>
        {
            var (a, b) = InputUtil.ReadLine<int>();
        });
    }

#if TESTSTRING
    [Theory]
    [InlineData("abc def", "abc", "def")]
    [InlineData("cat dog", "cat", "dog")]
    [InlineData("hello world", "hello", "world")]
    [InlineData("‚ ‚¢‚¤ ‚©‚«‚­‚¯‚±", "‚ ‚¢‚¤", "‚©‚«‚­‚¯‚±")]
    public void TestTwoStringInput(string input, string firstWord, string secondWord)
    {
        RedirectStdIn(input);
        var (a, b) = InputUtil.ReadLineString();
        Assert.Equal(firstWord, a);
        Assert.Equal(secondWord, b);
    }

    // this is not required behavior, but it is desirable
    [Fact]
    public void TestTwoStringInputTrailingSpace()
    {
        RedirectStdIn("abc def ");
        var (a, b) = InputUtil.ReadLineString();
        Assert.Equal("abc", a);
        Assert.Equal("def", b);
    }

    // this is not required behavior, but it is desirable
    [Fact]
    public void TestTwoStringInputExtraData()
    {
        RedirectStdIn("abc def ghi");
        var (a, b) = InputUtil.ReadLineString();
        Assert.Equal("abc", a);
        Assert.Equal("def", b);
    }

    [Fact]
    public void TestNotEnoughStringInput()
    {
        RedirectStdIn("abc");
        Assert.Throws<FormatException>(() =>
        {
            var (a, b) = InputUtil.ReadLineString();
        });
    }

#endif

    [Theory]
    [InlineData("1 2 3 4 5", new[] { 1, 2, 3, 4, 5 })]
    [InlineData("1 10 100", new[] { 1, 10, 100 })]
    [InlineData("0", new[] { 0 })]
    public void TestForeach(string input, int[] expected)
    {
        RedirectStdIn(input);
        var count = 0;
        foreach (var i in InputUtil.ReadLine<int>())
        {
            Assert.Equal(expected[count++], i);
        }
        Assert.Equal(expected.Length, count);
    }

    [Fact]
    public void TestForeachEmpty()
    {
        RedirectStdIn("");
        foreach (var _ in InputUtil.ReadLine<int>())
        {
            Assert.Fail("");
        }
    }

#if TESTSTRING
    [Theory]
    [InlineData("abc def ghi", new[] { "abc", "def", "ghi" })]
    [InlineData("‚ ‚¢‚¤ ‚©‚«‚­‚¯‚±", new[] { "‚ ‚¢‚¤", "‚©‚«‚­‚¯‚±" })]
    [InlineData("a", new[] { "a" })]
    public void TestForeachString(string input, string[] expected)
    {
        RedirectStdIn(input);
        var count = 0;
        foreach (var i in InputUtil.ReadLineString())
        {
            Assert.Equal(expected[count++], i);
        }
        Assert.Equal(expected.Length, count);
    }

    [Fact]
    public void TestForeachEmptyString()
    {
        RedirectStdIn("");
        foreach (var _ in InputUtil.ReadLineString())
        {
            Assert.Fail("");
        }
    }
#endif

    [Fact]
    public void TestForeachIdempotent()
    {
        RedirectStdIn("1 2 3 4 5");
        var token = InputUtil.ReadLine<int>();
        var count = 1;
        foreach (var i in token)
        {
            Assert.Equal(count++, i);
        }
        Assert.Equal(6, count);
        count = 1;
        foreach (var i in token)
        {
            Assert.Equal(count++, i);
        }
        Assert.Equal(6, count);
    }

    [Fact]
    public void TestForeachIdempotentString()
    {
        RedirectStdIn("abc def ghi");
        var token = InputUtil.ReadLineString();
        var expected = new[] { "abc", "def", "ghi" };
        var count = 0;
        foreach (var i in token)
        {
            Assert.Equal(expected[count++], i);
        }
        Assert.Equal(3, count);
        count = 0;
        foreach (var i in token)
        {
            Assert.Equal(expected[count++], i);
        }
        Assert.Equal(3, count);
    }

    [Fact]
    public void TestToArray()
    {
        RedirectStdIn("1 2 3 4 5");
        var token = InputUtil.ReadLine<int>();
        var array = token.ToArray();
        Assert.Equal(5, array.Length);
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(3, array[2]);
        Assert.Equal(4, array[3]);
        Assert.Equal(5, array[4]);
    }

    [Theory]
    [InlineData("1 2 3 4 5", new[] { 1, 2, 3, 4, 5 })]
    [InlineData("1 10 100", new[] { 1, 10, 100 })]
    [InlineData("0", new[] { 0 })]
    public void TestToArrayEqual(string input, int[] expected)
    {
        RedirectStdIn(input);
        var token = InputUtil.ReadLine<int>();
        var array = token.ToArray();
        Assert.Equal(expected.Length, array.Length);
        Assert.True(expected.SequenceEqual(array));
    }

    [Fact]
    public void TestToArrayEmpty()
    {
        RedirectStdIn("");
        var token = InputUtil.ReadLine<int>();
        var array = token.ToArray();
        Assert.Empty(array);
    }

    [Fact]
    public void TestToArrayTrailingSpace()
    {
        RedirectStdIn("1 2 3 ");
        var token = InputUtil.ReadLine<int>();
        var array = token.ToArray();
        Assert.Equal(3, array.Length);
        Assert.True(new[] { 1, 2, 3 }.SequenceEqual(array));
    }

#if TESTSTRING
    [Fact]
    public void TestToArrayString()
    {
        RedirectStdIn("abc def ghi");
        var token = InputUtil.ReadLineString();
        var array = token.ToArray();
        Assert.Equal(3, array.Length);
        Assert.Equal("abc", array[0]);
        Assert.Equal("def", array[1]);
        Assert.Equal("ghi", array[2]);
    }

    [Theory]
    [InlineData("abc def ghi", new[] { "abc", "def", "ghi" })]
    [InlineData("‚ ‚¢‚¤ ‚©‚«‚­‚¯‚±", new[] { "‚ ‚¢‚¤", "‚©‚«‚­‚¯‚±" })]
    [InlineData("a", new[] { "a" })]
    public void TestToArrayEqualString(string input, string[] expected)
    {
        RedirectStdIn(input);
        var token = InputUtil.ReadLineString();
        var array = token.ToArray();
        Assert.Equal(expected.Length, array.Length);
        Assert.True(expected.SequenceEqual(array));
    }

    [Fact]
    public void TestToArrayEmptyString()
    {
        RedirectStdIn("");
        var token = InputUtil.ReadLineString();
        var array = token.ToArray();
        Assert.Empty(array);
    }

    [Fact]
    public void TestToArrayTrailingSpaceString()
    {
        RedirectStdIn("abc def ghi ");
        var token = InputUtil.ReadLineString();
        var array = token.ToArray();
        Assert.Equal(3, array.Length);
        Assert.True(new[] { "abc", "def", "ghi" }.SequenceEqual(array));
    }
#endif

    [Fact]
    public void TestLargeInput()
    {
        var str = string.Join(" ", Enumerable.Range(0, 100000));
        RedirectStdIn(str);
        var token = InputUtil.ReadLine<int>();
        var count = 0;
        foreach (var i in token)
        {
            Assert.Equal(count++, i);
        }
        Assert.Equal(100000, count);
    }

    [Fact]
    public void TestLargeInputString()
    {
        var str = string.Join(" ", Enumerable.Range(0, 100000).Select(x => x.ToString()));
        RedirectStdIn(str);
        var token = InputUtil.ReadLineString();
        var count = 0;
        foreach (var i in token)
        {
            Assert.Equal(count++.ToString(), i);
        }
    }

    [Fact]
    public void TestLinqSelect()
    {
        RedirectStdIn("1 2 3 4 5");
        var array = InputUtil.ReadLine<int>().Select(x => x * x).ToArray();
        Assert.Equal(5, array.Length);
        Assert.True(new[] { 1, 4, 9, 16, 25 }.SequenceEqual(array));
    }

    [Fact]
    public void TestLinqWhere()
    {
        RedirectStdIn("1 2 3 4 5");
        var array = InputUtil.ReadLine<int>().Where(x => x % 2 == 0).ToArray();
        Assert.Equal(2, array.Length);
        Assert.True(new[] { 2, 4 }.SequenceEqual(array));
    }

    [Theory]
    [InlineData("1,2,3,4,5", new[] { 1, 2, 3, 4, 5 })]
    [InlineData("1,10,100", new[] { 1, 10, 100 })]
    public void TestCustomSeparator(string input, int[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c) = InputUtil.ReadLine<int>(CultureInfo.InvariantCulture, ',');
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
    }

    [Theory]
    [InlineData("abc,def,ghi", new[] { "abc", "def", "ghi" })]
    [InlineData("‚ ‚¢‚¤,‚©‚«‚­‚¯‚±,‚³", new[] { "‚ ‚¢‚¤", "‚©‚«‚­‚¯‚±", "‚³" })]
    public void TestCustomSeparatorString(string input, string[] expected)
    {
        RedirectStdIn(input);
        var (a, b, c) = InputUtil.ReadLineString(CultureInfo.InvariantCulture, ',');
        Assert.Equal(expected[0], a);
        Assert.Equal(expected[1], b);
        Assert.Equal(expected[2], c);
    }
}