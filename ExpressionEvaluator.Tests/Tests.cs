// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftCircuits.ExpressionEvaluator;
using System;

namespace ExpressionEvaluatorTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestBasic()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();

            // Numeric evaluation
            Assert.AreEqual(5, eval.Evaluate("5"));
            Assert.AreEqual(-5, eval.Evaluate("-5"));
            Assert.AreEqual(12345.6, eval.Evaluate("12345.6"));
            Assert.AreEqual("abc", eval.Evaluate("\"abc\""));

            // Operators
            Assert.AreEqual(5, eval.Evaluate("2 + 3"));
            Assert.AreEqual(-1, eval.Evaluate("2 - 3"));
            Assert.AreEqual(6, eval.Evaluate("2 * 3"));
            Assert.AreEqual(0, eval.Evaluate("2 / 3"));
            Assert.AreEqual(0.667, Math.Round(eval.Evaluate("2.0 / 3").ToDouble(), 3));
            Assert.AreEqual(2, eval.Evaluate("2 % 3"));
            Assert.AreEqual(23, eval.Evaluate("2 & 3"));

            // Expressions
            Assert.AreEqual(17, eval.Evaluate("2 + 3 * 5"));
            Assert.AreEqual(25, eval.Evaluate("(2 + 3) * 5"));
            Assert.AreEqual(-25, eval.Evaluate("(2 + 3) * -5"));
            Assert.AreEqual(22.0, eval.Evaluate("(2 + 3) * (-5 + 14) / 2"));
            Assert.AreEqual(22.5, eval.Evaluate("(2.0 + 3) * (-5 + 14) / 2"));
            Assert.AreEqual(22.0, eval.Evaluate("((2 + 3) * (-5 + 14)) / 2"));
            Assert.AreEqual(22.5, eval.Evaluate("((2.0 + 3) * (-5 + 14)) / 2"));

            // Excess whitespace
            Assert.AreEqual(5, eval.Evaluate("     5    "));
            Assert.AreEqual(12345.6, eval.Evaluate("     12345.6    "));
            Assert.AreEqual(5, eval.Evaluate("     2     +     3     "));
            Assert.AreEqual(5, eval.Evaluate(" \t\r\n2 \t\r\n+ \t\r\n3 \t\r\n"));
        }

        [TestMethod]
        public void TestStrings()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();

            Assert.AreEqual(123, eval.Evaluate("123"));
            Assert.AreEqual(123.0, eval.Evaluate("123"));
            Assert.AreEqual("123", eval.Evaluate("123"));
            Assert.AreEqual(123, eval.Evaluate("\"123\""));
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

            Assert.IsTrue(eval.Evaluate("'abc' + 5") == 5);
            Assert.IsTrue(eval.Evaluate("'abc' - 5") == -5);
            Assert.IsTrue(eval.Evaluate("'abc' * 5") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' / 5") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' % 5") == 0);
            Assert.IsTrue(eval.Evaluate("-'abc'") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' & 5") == "abc5");

            Assert.IsTrue(eval.Evaluate("5 + 'abc'") == 5);
            Assert.IsTrue(eval.Evaluate("5 - 'abc'") == 5);
            Assert.IsTrue(eval.Evaluate("5 * 'abc'") == 0);
            Assert.IsTrue(eval.Evaluate("5 / 'abc'") == 0);
            Assert.IsTrue(eval.Evaluate("5 % 'abc'") == 0);
            Assert.IsTrue(eval.Evaluate("5 & 'abc'") == "5abc");
        }

        [TestMethod]
        public void TestVariableConversions()
        {
            Variable v = new Variable(12345.6);

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
            Variable v = new Variable(100);
            Variable v2 = new Variable(34);

            // Addition
            Assert.IsTrue((v + 1) == 101);
            Assert.IsTrue((v + 1.5) == 101.5);
            Assert.IsTrue((v + "10") == 110);
            Assert.IsTrue((v + "abc") == 100);
            Assert.IsTrue((v + v2) == 134);

            // Subtraction
            Assert.IsTrue((v - 1) == 99);
            Assert.IsTrue((v - 1.5) == 98.5);
            Assert.IsTrue((v - "10") == 90);
            Assert.IsTrue((v - "abc") == 100);
            Assert.IsTrue((v - v2) == 66);

            // Multiplication
            Assert.IsTrue((v * 1) == 100);
            Assert.IsTrue((v * 1.5) == 150);
            Assert.IsTrue((v * "10") == 1000);
            Assert.IsTrue((v * "abc") == 0);
            Assert.IsTrue((v * v2) == 3400);

            // Division
            Assert.IsTrue((v / 1) == 100);
            Assert.IsTrue((v / 1.5) == 66.66666666666667);
            Assert.IsTrue((v / "10") == 10);
            Assert.IsTrue((v / 0) == 0);
            Assert.IsTrue((v / "abc") == 0);
            Assert.IsTrue((v / v2) == 2);

            // Modulus
            Assert.IsTrue((v % 1) == 0);
            Assert.IsTrue((v % 1.5) == 1);
            Assert.IsTrue((v % "10") == 0);
            Assert.IsTrue((v % 0) == 0);
            Assert.IsTrue((v % "abc") == 0);
            Assert.IsTrue((v % v2) == 32);

            // Concatenation
            Assert.IsTrue((v & 1) == "1001");
            Assert.IsTrue((v & 1.5) == 1001.5);
            Assert.IsTrue((v & "10") == "10010");
            Assert.IsTrue((v & "abc") == "100abc");
            Assert.IsTrue((v & v2) == 10034);

            // Negation
            Assert.IsTrue((-v) == -100);
        }

        [TestMethod]
        public void TestVariableComparisonOperators()
        {
            Variable v = new Variable(100);

            // Equals
            Assert.IsTrue(v == v.ToInteger());
            Assert.IsTrue(v == v.ToDouble());
            Assert.IsTrue(v == v.ToString());
            Assert.IsTrue(v == 100);
            Assert.IsTrue(v == 100.0);
            Assert.IsTrue(v == "100");
            Assert.IsFalse(v == 101);
            Assert.IsFalse(v == 101.1);
            Assert.IsFalse(v == "101");
            Assert.IsFalse(v == "abc");  // Compare as strings

            // Not equals
            Assert.IsFalse(v != v.ToInteger());
            Assert.IsFalse(v != v.ToDouble());
            Assert.IsFalse(v != v.ToString());
            Assert.IsTrue(v != 101);
            Assert.IsTrue(v != 101.1);
            Assert.IsTrue(v != "101");
            Assert.IsFalse(v != 100);
            Assert.IsFalse(v != 100.0);
            Assert.IsFalse(v != "100");
            Assert.IsTrue(v != "abc");  // Compare as strings

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
            Assert.IsTrue(v == "abc");
            Assert.IsFalse(v == "def");
            Assert.IsTrue(v != "def");
            Assert.IsFalse(v != "abc");
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

        [TestMethod]
        public void TestSymbols()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.IsTrue(eval.Evaluate("two + two") == 4);
            Assert.IsTrue(eval.Evaluate("TWO + THREE * FIVE") == 17);
            Assert.IsTrue(eval.Evaluate("(two + three) * -five") == -25);
        }

        [TestMethod]
        public void TestFunctions()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.IsTrue(eval.Evaluate("add(two, two)") == 4);
            Assert.IsTrue(eval.Evaluate("TWO + multiply(THREE, FIVE)") == 17);
            Assert.IsTrue(eval.Evaluate("-multiply(add(two, three), five)") == -25);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestSyntaxErrorException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.Evaluate("2 + ");
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestUndefinedSymbolException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.Evaluate("two + two");
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestUndefinedFunctionException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.Evaluate("f() + 2");
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestWrongParameterCountException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.Evaluate("add(1, 2, 3)");
        }

        private void Eval_EvaluateSymbol(object sender, SymbolEventArgs e)
        {
            switch (e.Name.ToUpper())
            {
                case "TWO":
                    e.Result.SetValue(2);
                    break;
                case "THREE":
                    e.Result.SetValue(3);
                    break;
                case "FIVE":
                    e.Result.SetValue(5);
                    break;
                default:
                    e.Status = SymbolStatus.UndefinedSymbol;
                    break;
            }
        }

        private void Eval_EvaluateFunction(object sender, FunctionEventArgs e)
        {
            switch (e.Name.ToUpper())
            {
                case "ADD":
                    if (e.Parameters.Length == 2)
                    {
                        e.Parameters[0].Add(e.Parameters[1]);
                        e.Result.SetValue(e.Parameters[0]);
                    }
                    else e.Status = FunctionStatus.WrongParameterCount;
                    break;
                case "MULTIPLY":
                    if (e.Parameters.Length == 2)
                    {
                        e.Parameters[0].Multiply(e.Parameters[1]);
                        e.Result.SetValue(e.Parameters[0]);
                    }
                    else e.Status = FunctionStatus.WrongParameterCount;
                    break;
                default:
                    e.Status = FunctionStatus.UndefinedFunction;
                    break;
            }
        }
    }
}
