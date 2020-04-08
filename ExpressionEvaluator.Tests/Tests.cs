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
            Assert.IsTrue(eval.Evaluate("2 + 3") == 5);
            Assert.IsTrue(eval.Evaluate("2 - 3") == -1);
            Assert.IsTrue(eval.Evaluate("2 * 3") == 6);
            Assert.IsTrue(eval.Evaluate("2 / 3") == 0);
            Assert.IsTrue(Math.Round(eval.Evaluate("2.0 / 3").ToDouble(), 3) == 0.667);
            Assert.IsTrue(eval.Evaluate("2 % 3") == 2);
            Assert.IsTrue(eval.Evaluate("2 % 3") == "2");
            Assert.IsTrue(eval.Evaluate(" \t\r\n2 \t\r\n+ \t\r\n3 \t\r\n") == 5);
            Assert.IsTrue(eval.Evaluate("     2     %     3     ") == 2);

            // Test expressions
            Assert.IsTrue(eval.Evaluate("2 + 3 * 5") == 17);
            Assert.IsTrue(eval.Evaluate("(2 + 3) * 5") == 25);
            Assert.IsTrue(eval.Evaluate("(2 + 3) * -5") == -25);
            Assert.IsTrue(eval.Evaluate("(2 + 3) * (-5 + 14) / 2") == 22.0);
            Assert.IsTrue(eval.Evaluate("(2.0 + 3) * (-5 + 14) / 2") == 22.5);
            Assert.IsTrue(eval.Evaluate("((2 + 3) * (-5 + 14)) / 2") == 22.0);
            Assert.IsTrue(eval.Evaluate("((2.0 + 3) * (-5 + 14)) / 2") == 22.5);
        }

        [TestMethod]
        public void TestStrings()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            Assert.IsTrue(eval.Evaluate("12 & 34") == 1234);
            Assert.IsTrue(eval.Evaluate("\"12\" & \"34\"") == "1234");
            Assert.IsTrue(eval.Evaluate("'12' + '34'") == 46);
            Assert.IsTrue(eval.Evaluate("'abc' + 'def'") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' & 'def'") == "abcdef");

            Assert.IsTrue(eval.Evaluate("16") == "16.00");
            Assert.IsTrue(eval.Evaluate("16.00") == "16");
            Assert.IsTrue(eval.Evaluate("'16'") == "16.00");
            Assert.IsTrue(eval.Evaluate("'16.00'") == "16");
            Assert.IsFalse(eval.Evaluate("16") != "16.00");
            Assert.IsFalse(eval.Evaluate("16.00") != "16");
            Assert.IsFalse(eval.Evaluate("'16'") != "16.00");
            Assert.IsFalse(eval.Evaluate("'16.00'") != "16");

            Assert.IsTrue(eval.Evaluate("16") == 16.0);
            Assert.IsTrue(eval.Evaluate("16.00") == 16);
            Assert.IsTrue(eval.Evaluate("'16'") == 16.0);
            Assert.IsTrue(eval.Evaluate("'16.00'") == 16);
            Assert.IsFalse(eval.Evaluate("16") != 16.0);
            Assert.IsFalse(eval.Evaluate("16.00") != 16);
            Assert.IsFalse(eval.Evaluate("'16'") != 16.0);
            Assert.IsFalse(eval.Evaluate("'16.00'") != 16);

            Assert.IsTrue(eval.Evaluate("'abc' + 5") == 5);
            Assert.IsTrue(eval.Evaluate("'abc' - 5") == -5);
            Assert.IsTrue(eval.Evaluate("'abc' * 5") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' / 5") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' % 5") == 0);
            Assert.IsTrue(eval.Evaluate("-'abc'") == 0);
            Assert.IsTrue(eval.Evaluate("'abc' & 5") == "abc5");

            Assert.IsTrue(eval.Evaluate("123") == 123);
            Assert.IsTrue(eval.Evaluate("\"123\"") == 123);
            Assert.IsTrue(eval.Evaluate("123") == "123");
            Assert.IsTrue(eval.Evaluate("\"123\"") == "123");

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
            Assert.IsTrue(12346 == v);
            Assert.IsTrue(12345.6 == v);
            Assert.IsTrue("12345.6" == v);
            Assert.IsTrue(v == 12346);
            Assert.IsTrue(v == 12345.6);
            Assert.IsTrue(v == "12345.6");
            Assert.IsFalse(12346 != v);
            Assert.IsFalse(12345.6 != v);
            Assert.IsFalse("12345.6" != v);

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
            Assert.IsTrue(v2 == 101);
            v2 = v + 1.5;
            Assert.IsTrue(v2 == 101.5);
            v2 = v + "10";
            Assert.IsTrue(v2 == 110);
            v2 = v + v2;
            Assert.IsTrue(v2 == 210);

            v2 = v - 1;
            Assert.IsTrue(v2 == 99);
            v2 = v - 1.5;
            Assert.IsTrue(v2 == 98.5);
            v2 = v - "10";
            Assert.IsTrue(v2 == 90);
            v2 = v - v2;
            Assert.IsTrue(v2 == 10);

            v2 = v * 1;
            Assert.IsTrue(v2 == 100);
            v2 = v * 1.5;
            Assert.IsTrue(v2 == 150);
            v2 = v * "10";
            Assert.IsTrue(v2 == 1000);
            v2 = v * v2;
            Assert.IsTrue(v2 == 100000);

            v2 = v / 1;
            Assert.IsTrue(v2 == 100);
            v2 = v / 1.5;
            Assert.IsTrue(v2 == 66.66666666666667);
            v2 = v / "10";
            Assert.IsTrue(v2 == 10);
            v2 = v / v2;
            Assert.IsTrue(v2 == 10);
            v2 = v / 0;
            Assert.IsTrue(v2 == 0);

            v2 = v % 1;
            Assert.IsTrue(v2 == 0);
            v2 = v % 1.5;
            Assert.IsTrue(v2 == 1);
            v2 = v % "34";
            Assert.IsTrue(v2 == 32);
            v2 = v % v2;
            Assert.IsTrue(v2 == 4);
            v2 = v % 0;
            Assert.IsTrue(v2 == 0);

            v2 = v & 1;
            Assert.IsTrue(v2 == "1001");
            v2 = v & 1.5;
            Assert.IsTrue(v2 == 1001.5);
            v2 = v & "10";
            Assert.IsTrue(v2 == 10010);
            v2 = v & v2;
            Assert.IsTrue(v2 == 10010010);

            v2 = -v;
            Assert.IsTrue(v2 == -100);
        }

        [TestMethod]
        public void TestSymbols()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.IsTrue(eval.Evaluate("two + two") == 4);
            Assert.IsTrue(eval.Evaluate("two + three * five") == 17);
            Assert.IsTrue(eval.Evaluate("(two + three) * -five") == -25);
        }

        [TestMethod]
        public void TestFunctions()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.IsTrue(eval.Evaluate("add(two, two)") == 4);
            Assert.IsTrue(eval.Evaluate("two + multiply(three, five)") == 17);
            Assert.IsTrue(eval.Evaluate("-multiply(add(two, three), five)") == -25);
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
