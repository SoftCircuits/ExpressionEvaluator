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

            // Test operators
            Assert.AreEqual(5, eval.Evaluate("2 + 3").ToInteger());
            Assert.AreEqual(-1, eval.Evaluate("2 - 3").ToInteger());
            Assert.AreEqual(6, eval.Evaluate("2 * 3").ToInteger());
            Assert.AreEqual(0.667, Math.Round(eval.Evaluate("2.0 / 3").ToDouble(), 3));
            Assert.AreEqual(2, eval.Evaluate("2 % 3").ToInteger());
            Assert.AreEqual("2", eval.Evaluate("2 % 3").ToString());

            // Test expressions
            Assert.AreEqual(17, eval.Evaluate("2 + 3 * 5").ToInteger());
            Assert.AreEqual(25, eval.Evaluate("(2 + 3) * 5").ToInteger());
            Assert.AreEqual(-25, eval.Evaluate("(2 + 3) * -5").ToInteger());
            Assert.AreEqual(22, eval.Evaluate("(2 + 3) * (-5 + 14) / 2").ToDouble());
            Assert.AreEqual(22.5, eval.Evaluate("(2.0 + 3) * (-5 + 14) / 2").ToDouble());
            Assert.AreEqual(22, eval.Evaluate("((2 + 3) * (-5 + 14)) / 2").ToDouble());
            Assert.AreEqual(22.5, eval.Evaluate("((2.0 + 3) * (-5 + 14)) / 2").ToDouble());
        }

        [TestMethod]
        public void TestStrings()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            Assert.AreEqual("1234", eval.Evaluate("\"12\" & \"34\"").ToString());
            Assert.AreEqual(46, eval.Evaluate("'12' + '34'").ToInteger());
            Assert.AreEqual("0", eval.Evaluate("'abc' + 'def'").ToString());
            Assert.AreEqual("abcdef", eval.Evaluate("'abc' & 'def'").ToString());

            Assert.AreEqual(1, eval.Evaluate("'abc' + 1"));
            Assert.AreEqual(-1, eval.Evaluate("'abc' - 1"));
            Assert.AreEqual(0, eval.Evaluate("'abc' * 1"));
            Assert.AreEqual(0, eval.Evaluate("'abc' / 1"));
            Assert.AreEqual(0, eval.Evaluate("'abc' % 1"));
            Assert.AreEqual("abc", eval.Evaluate("-'abc'"));
            Assert.AreEqual("abc1", eval.Evaluate("'abc' & 1"));

            Assert.AreEqual(VariableType.Integer, eval.Evaluate("\"2\" + \"2\"").Type);
            Assert.AreEqual(VariableType.Double, eval.Evaluate("\"2.5\" + \"2.6\"").Type);
            Assert.AreEqual(VariableType.String, eval.Evaluate("2.5 & 2.6").Type);
        }

        [TestMethod]
        public void TestVariableOperators()
        {
            Variable v = new Variable();
            Variable v2;

            // Conversion operators
            v.SetValue("12345.6");
            Assert.AreEqual(12346, v);
            Assert.AreEqual(12345.6, v);
            Assert.AreEqual("12345.6", v);

            // Comparison operators
            v.SetValue(100);

            Assert.IsTrue(v == 100);
            Assert.IsFalse(v == 47);
            Assert.IsTrue(v != 1087);
            Assert.IsFalse(v != 100);
            Assert.IsTrue(v < 107);
            Assert.IsFalse(v < 38);
            Assert.IsTrue(v <= 100);
            Assert.IsFalse(v <= 9);
            Assert.IsTrue(v > 39);
            Assert.IsFalse(v > 2473);
            Assert.IsTrue(v >= 100);
            Assert.IsFalse(v >= 3980);

            v.SetValue("abc");

            Assert.IsTrue(v == "abc");
            Assert.IsFalse(v == "def");
            Assert.IsTrue(v != "def");
            Assert.IsFalse(v != "abc");
            Assert.IsTrue(v < "def");
            Assert.IsFalse(v < "abc");
            Assert.IsTrue(v <= "abc");
            Assert.IsFalse(v <= "aaa");
            Assert.IsTrue(v > "aaa");
            Assert.IsFalse(v > "abc");
            Assert.IsTrue(v >= "abc");
            Assert.IsFalse(v >= "xyz");

            Assert.IsTrue(v > "100");   // Compare as strings
            Assert.IsFalse(v > 100);    // Compare as integers ("abc" == 0)

            v.SetValue("100");
            Assert.IsTrue(v > "10");    // Compare as integers
            Assert.IsTrue(v < 200);     // Compare as integers
            Assert.IsTrue(v < "100.1"); // Compare as doubles
            Assert.IsTrue(v < 100.1);   // Compare as doubles

            // Operation operators
            v.SetValue(100);

            v2 = v + 1;
            Assert.AreEqual(101, v2);
            v2 = v + 1.5;
            Assert.AreEqual(101.5, v2);
            v2 = v + "10";
            Assert.AreEqual(110, v2);
            v2 = v + v2;
            Assert.AreEqual(210, v2);

            v2 = v - 1;
            Assert.AreEqual(99, v2);
            v2 = v - 1.5;
            Assert.AreEqual(98.5, v2);
            v2 = v - "10";
            Assert.AreEqual(90, v2);
            v2 = v - v2;
            Assert.AreEqual(10, v2);

            v2 = v * 1;
            Assert.AreEqual(100, v2);
            v2 = v * 1.5;
            Assert.AreEqual(150, v2);
            v2 = v * "10";
            Assert.AreEqual(1000, v2);
            v2 = v * v2;
            Assert.AreEqual(100000, v2);

            v2 = v / 1;
            Assert.AreEqual(100, v2);
            v2 = v / 1.5;
            Assert.AreEqual(66.66666666666667, v2);
            v2 = v / "10";
            Assert.AreEqual(10, v2);
            v2 = v / v2;
            Assert.AreEqual(10, v2);
            v2 = v / 0;
            Assert.AreEqual(0, v2);

            v2 = v % 1;
            Assert.AreEqual(0, v2);
            v2 = v % 1.5;
            Assert.AreEqual(1, v2);
            v2 = v % "34";
            Assert.AreEqual(32, v2);
            v2 = v % v2;
            Assert.AreEqual(4, v2);
            v2 = v % 0;
            Assert.AreEqual(0, v2);

            v2 = v & 1;
            Assert.AreEqual("1001", v2);
            v2 = v & 1.5;
            Assert.AreEqual(1001.5, v2);
            v2 = v & "10";
            Assert.AreEqual(10010, v2);
            v2 = v & v2;
            Assert.AreEqual(10010010, v2);

            v2 = -v;
            Assert.AreEqual(-100, v2);
        }

        [TestMethod]
        public void TestSymbols()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.AreEqual(4, eval.Evaluate("two + two").ToInteger());
            Assert.AreEqual(17, eval.Evaluate("two + three * five").ToInteger());
            Assert.AreEqual(-25, eval.Evaluate("(two + three) * -five").ToInteger());
        }

        [TestMethod]
        public void TestFunctions()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.AreEqual(4, eval.Evaluate("add(two, two)").ToInteger());
            Assert.AreEqual(17, eval.Evaluate("two + multiply(three, five)").ToInteger());
            Assert.AreEqual(-25, eval.Evaluate("-multiply(add(two, three), five)").ToInteger());
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestSyntaxException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.Evaluate("2 +");
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestBadSymbolException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.Evaluate("two + two");
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestBadFunctionException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.Evaluate("f() + 2");
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionException))]
        public void TestBadArgumentsException()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.Evaluate("add(1, 2, 3)");
        }

        private void Eval_EvaluateSymbol(object sender, SymbolEventArgs e)
        {
            switch (e.Name)
            {
                case "two":
                    e.Result.SetValue(2);
                    break;
                case "three":
                    e.Result.SetValue(3);
                    break;
                case "five":
                    e.Result.SetValue(5);
                    break;
                default:
                    e.Status = SymbolStatus.UndefinedSymbol;
                    break;
            }
        }

        private void Eval_EvaluateFunction(object sender, FunctionEventArgs e)
        {
            switch (e.Name)
            {
                case "add":
                    if (e.Parameters.Length == 2)
                    {
                        e.Parameters[0].Add(e.Parameters[1]);
                        e.Result.SetValue(e.Parameters[0]);
                    }
                    else
                        e.Status = FunctionStatus.WrongParameterCount;
                    break;
                case "multiply":
                    if (e.Parameters.Length == 2)
                    {
                        e.Parameters[0].Multiply(e.Parameters[1]);
                        e.Result.SetValue(e.Parameters[0]);
                    }
                    else
                        e.Status = FunctionStatus.WrongParameterCount;
                    break;
                default:
                    e.Status = FunctionStatus.UndefinedFunction;
                    break;
            }
        }
    }
}
