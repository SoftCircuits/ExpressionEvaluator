// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.ExpressionEvaluator;

namespace ExpressionEvaluatorTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestBasic()
        {
            ExpressionEvaluator eval = new();

            // Operand evaluation
            Assert.AreEqual(5, eval.Evaluate("5"));
            Assert.AreEqual(-34, eval.Evaluate("-34"));
            Assert.AreEqual(12345.6, eval.Evaluate("12345.6"));
            Assert.AreEqual("abc", eval.Evaluate("\"abc\""));
            Assert.AreEqual("abc", eval.Evaluate("'abc'"));

            // Operators
            Assert.AreEqual(5, eval.Evaluate("2 + 3"));
            Assert.AreEqual(-1, eval.Evaluate("2 - 3"));
            Assert.AreEqual(6, eval.Evaluate("2 * 3"));
            Assert.AreEqual(0, eval.Evaluate("2 / 3"));
            double result = Math.Round(eval.Evaluate("2.0 / 3").ToDouble(), 5);
            Assert.AreEqual("0.66667", result.ToString());
            Assert.AreEqual(2, eval.Evaluate("2 % 3"));
            Assert.AreEqual(8, eval.Evaluate("2 ^ 3"));
            Assert.AreEqual(23, eval.Evaluate("2 & 3"));

            // Expressions
            Assert.AreEqual(17, eval.Evaluate("2 + 3 * 5"));
            Assert.AreEqual(25, eval.Evaluate("(2 + 3) * 5"));
            Assert.AreEqual(-25, eval.Evaluate("(2 + 3) * -5"));
            Assert.AreEqual(22.0, eval.Evaluate("(2 + 3) * (-5 + 14) / 2"));
            Assert.AreEqual(22.5, eval.Evaluate("(2.0 + 3) * (-5 + 14) / 2"));
            Assert.AreEqual(20.0, eval.Evaluate("(2 + 3) * ((-5 + 14) / 2)"));
            Assert.AreEqual(20.0, eval.Evaluate("(2.0 + 3) * ((-5 + 14) / 2)"));

            // Excess whitespace
            Assert.AreEqual(5, eval.Evaluate("     5    "));
            Assert.AreEqual(12345.6, eval.Evaluate("     12345.6    "));
            Assert.AreEqual(5, eval.Evaluate("     2     +     3     "));
            Assert.AreEqual(5, eval.Evaluate(" \t\r\n2 \t\r\n+ \t\r\n3 \t\r\n"));
        }

        [TestMethod]
        public void TestStrings()
        {
            ExpressionEvaluator eval = new();

            Assert.AreEqual(123, eval.Evaluate("123"));
            Assert.AreEqual(123.0, eval.Evaluate("123"));
            Assert.AreEqual("123", eval.Evaluate("123"));
            Assert.AreEqual(123, eval.Evaluate("'123'"));
            Assert.AreEqual(123.0, eval.Evaluate("'123'"));
            Assert.AreEqual("123", eval.Evaluate("'123'"));
            Assert.AreEqual(123, eval.Evaluate("123.0"));
            Assert.AreEqual(123.0, eval.Evaluate("123.0"));
            Assert.AreEqual("123", eval.Evaluate("123.0"));
            Assert.AreEqual(123, eval.Evaluate("'123.0'"));
            Assert.AreEqual(123.0, eval.Evaluate("'123.0'"));
            Assert.AreEqual("123.0", eval.Evaluate("'123.0'"));

            Assert.AreEqual(VariableType.Integer, eval.Evaluate("\"2\" + \"2\"").Type);
            Assert.AreEqual(VariableType.Double, eval.Evaluate("\"2.5\" + \"2.6\"").Type);
            Assert.AreEqual(VariableType.String, eval.Evaluate("\"2\" & \"2\"").Type);
            Assert.AreEqual(VariableType.String, eval.Evaluate("2.5 & 2.6").Type);

            Assert.AreEqual(4, eval.Evaluate("\"2\" + \"2\""));
            Assert.AreEqual(5.1, eval.Evaluate("\"2.5\" + \"2.6\""));
            Assert.AreEqual("22", eval.Evaluate("\"2\" & \"2\""));
            Assert.AreEqual("2.52.6", eval.Evaluate("2.5 & 2.6"));

            Assert.AreEqual(5, eval.Evaluate("'abc' + 5"));
            Assert.AreEqual(-5, eval.Evaluate("'abc' - 5"));
            Assert.AreEqual(0, eval.Evaluate("'abc' * 5"));
            Assert.AreEqual(0, eval.Evaluate("'abc' / 5"));
            Assert.AreEqual(0, eval.Evaluate("'abc' % 5"));
            Assert.AreEqual(0, eval.Evaluate("-'abc'"));
            Assert.AreEqual(0, eval.Evaluate("'abc' ^ 5"));
            Assert.AreEqual("abc5", eval.Evaluate("'abc' & 5"));

            Assert.AreEqual(5, eval.Evaluate("5 + 'abc'"));
            Assert.AreEqual(5, eval.Evaluate("5 - 'abc'"));
            Assert.AreEqual(0, eval.Evaluate("5 * 'abc'"));
            Assert.AreEqual(0, eval.Evaluate("5 / 'abc'"));
            Assert.AreEqual(0, eval.Evaluate("5 % 'abc'"));
            Assert.AreEqual("5abc", eval.Evaluate("5 & 'abc'"));
        }

        [TestMethod]
        public void TestVariableConversions()
        {
            Variable v = new(12345.6);

            // Explicit conversions
            Assert.AreEqual(12346, v.ToInteger());
            Assert.AreEqual(12345.6, v.ToDouble());
            Assert.AreEqual("12345.6", v.ToString());

            // Compare explicit to implicit conversions
            Assert.AreEqual(v.ToInteger(), v);
            Assert.AreEqual(v.ToDouble(), v);
            Assert.AreEqual(v.ToString(), v);

            v.SetValue("abc");

            // Explicit conversions
            Assert.AreEqual(0, v.ToInteger());
            Assert.AreEqual(0.0, v.ToDouble());
            Assert.AreEqual("abc", v.ToString());

            // Compare explicit to implicit conversions
            Assert.AreEqual(v.ToInteger(), v);
            Assert.AreEqual(v.ToDouble(), v);
            Assert.AreEqual(v.ToString(), v);
        }

        [TestMethod]
        public void TestVariableOperators()
        {
            Variable v = new(100);
            Variable v2 = new(34);

            // Addition
            Assert.AreEqual(101, v + 1);
            Assert.AreEqual(101.5, v + 1.5);
            Assert.AreEqual(110, v + "10");
            Assert.AreEqual(100, v + "abc");
            Assert.AreEqual(134, v + v2);

            // Subtraction
            Assert.AreEqual(99, v - 1);
            Assert.AreEqual(98.5, v - 1.5);
            Assert.AreEqual(90, v - "10");
            Assert.AreEqual(100, v - "abc");
            Assert.AreEqual(66, v - v2);

            // Multiplication
            Assert.AreEqual(100, v * 1);
            Assert.AreEqual(150, v * 1.5);
            Assert.AreEqual(1000, v * "10");
            Assert.AreEqual(0, v * "abc");
            Assert.AreEqual(3400, v * v2);

            // Division
            Assert.AreEqual(100, v / 1);
            Assert.AreEqual(66.66666666666667, v / 1.5);
            Assert.AreEqual(10, v / "10");
            Assert.AreEqual(0, v / 0);
            Assert.AreEqual(0, v / "abc");
            Assert.AreEqual(2, v / v2);

            // Modulus
            Assert.AreEqual(0, v % 1);
            Assert.AreEqual(1, v % 1.5);
            Assert.AreEqual(0, v % "10");
            Assert.AreEqual(0, v % 0);
            Assert.AreEqual(0, v % "abc");
            Assert.AreEqual(32, v % v2);

            // Power
            v2.SetValue(v2.ToDouble());
            Assert.AreEqual(1000000, v ^ 3);
            Assert.AreEqual(100000, v ^ 2.5);
            Assert.AreEqual(10000, v ^ "2");
            Assert.AreEqual(1, v ^ 0);
            Assert.AreEqual(1, v ^ "abc");
            Assert.AreEqual(1E+68, v ^ v2);

            // Concatenation
            v2.SetValue(v2.ToInteger());
            Assert.AreEqual("1001", v & 1);
            Assert.AreEqual(1001.5, v & 1.5);
            Assert.AreEqual("10010", v & "10");
            Assert.AreEqual("100abc", v & "abc");
            Assert.AreEqual(10034, v & v2);

            // Negation
            Assert.AreEqual(-100, -v);
        }

        [TestMethod]
        public void TestVariableComparisonOperators()
        {
            Variable v = new(100);

            // Equals
            Assert.AreEqual(v.ToInteger(), v);
            Assert.AreEqual(v.ToDouble(), v);
            Assert.AreEqual(v.ToString(), v);
            Assert.AreEqual(100, v);
            Assert.AreEqual(100.0, v);
            Assert.AreEqual("100", v);
            Assert.AreNotEqual(101, v);
            Assert.AreNotEqual(101.1, v);
            Assert.AreNotEqual("101", v);
            Assert.AreNotEqual("abc", v);  // Compare as strings

            // Not equals
            Assert.AreEqual(v.ToInteger(), v);
            Assert.AreEqual(v.ToDouble(), v);
            Assert.AreEqual(v.ToString(), v);
            Assert.AreNotEqual(101, v);
            Assert.AreNotEqual(101.1, v);
            Assert.AreNotEqual("101", v);
            Assert.AreEqual(100, v);
            Assert.AreEqual(100.0, v);
            Assert.AreEqual("100", v);
            Assert.AreNotEqual("abc", v);  // Compare as strings

            // Less than
            Assert.IsFalse(v < v.ToInteger());
            Assert.IsFalse(v < v.ToDouble());
            Assert.IsFalse(v < v.ToString());
            Assert.IsTrue(v < 101);
            Assert.IsTrue(v < 100.1);
            Assert.IsTrue(v < "101");
            Assert.IsFalse(v < 99);
            Assert.IsFalse(v < 99.0);
            Assert.IsFalse(v < "99");
            Assert.IsTrue(v < "abc");  // Compare as strings

            // Less than or equal
            Assert.IsTrue(v <= v.ToInteger());
            Assert.IsTrue(v <= v.ToDouble());
            Assert.IsTrue(v <= v.ToString());
            Assert.IsTrue(v <= 100);
            Assert.IsTrue(v <= 100.1);
            Assert.IsTrue(v <= "100");
            Assert.IsFalse(v <= 99);
            Assert.IsFalse(v <= 99.0);
            Assert.IsFalse(v <= "99");
            Assert.IsTrue(v <= "abc");  // Compare as strings

            // Greater than
            Assert.IsFalse(v > v.ToInteger());
            Assert.IsFalse(v > v.ToDouble());
            Assert.IsFalse(v > v.ToString());
            Assert.IsTrue(v > 99);
            Assert.IsTrue(v > 99.9);
            Assert.IsTrue(v > "99");
            Assert.IsFalse(v > 101);
            Assert.IsFalse(v > 101.0);
            Assert.IsFalse(v > "101");
            Assert.IsFalse(v > "abc");  // Compare as strings

            // Greater than or equal
            Assert.IsTrue(v >= v.ToInteger());
            Assert.IsTrue(v >= v.ToDouble());
            Assert.IsTrue(v >= v.ToString());
            Assert.IsTrue(v >= 100);
            Assert.IsTrue(v >= 100.0);
            Assert.IsTrue(v >= "100");
            Assert.IsFalse(v >= 101);
            Assert.IsFalse(v >= 100.1);
            Assert.IsFalse(v >= "101");
            Assert.IsFalse(v >= "abc");  // Compare as strings

            v.SetValue("abc");

            // String comparisons
            Assert.AreEqual("abc", v);
            Assert.AreNotEqual("def", v);
            Assert.AreNotEqual("def", v);
            Assert.AreEqual("abc", v);
            Assert.IsTrue(v < "def");
            Assert.IsFalse(v < "101");
            Assert.IsTrue(v <= "abc");
            Assert.IsTrue(v <= "def");
            Assert.IsFalse(v <= "aaa");
            Assert.IsTrue(v > "aaa");
            Assert.IsFalse(v > "def");
            Assert.IsTrue(v >= "abc");
            Assert.IsTrue(v >= "aaa");
            Assert.IsFalse(v >= "def");

            // Comparisons do not round floats to integer
            v.SetValue("1.3");
            Assert.IsFalse(v == 1);

            // Comparisons do not evaluate non-numbers as 0
            v.SetValue("abc");
            Assert.IsFalse(v == 0);
        }
    }
}
