// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftCircuits.ExpressionEvaluator;

namespace TestExpressionEvaluator
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBasic()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            Assert.AreEqual(4, eval.Evaluate("2 + 2"));
            Assert.AreEqual(17, eval.Evaluate("2 + 3 * 5"));
            Assert.AreEqual(25, eval.Evaluate("(2 + 3) * 5"));
        }

        [TestMethod]
        public void TestSymbols()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.ProcessSymbol += Eval_ProcessSymbol;
            Assert.AreEqual(4, eval.Evaluate("two + two"));
            Assert.AreEqual(17, eval.Evaluate("two + three * five"));
            Assert.AreEqual(25, eval.Evaluate("(two + three) * five"));
        }

        [TestMethod]
        public void TestFunctions()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            eval.ProcessFunction += Eval_ProcessFunction;
            eval.ProcessSymbol += Eval_ProcessSymbol;
            Assert.AreEqual(4, eval.Evaluate("add(two, two)"));
            Assert.AreEqual(17, eval.Evaluate("two + multiply(three, five)"));
            Assert.AreEqual(25, eval.Evaluate("multiply(add(two, three), five)"));
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
            eval.ProcessFunction += Eval_ProcessFunction;
            eval.Evaluate("add(1, 2, 3)");
        }

        private void Eval_ProcessSymbol(object sender, SymbolEventArgs e)
        {
            switch (e.Name)
            {
                case "two":
                    e.Result = 2;
                    break;
                case "three":
                    e.Result = 3;
                    break;
                case "five":
                    e.Result = 5;
                    break;
                default:
                    e.Status = SymbolStatus.UndefinedSymbol;
                    break;
            }
        }

        private void Eval_ProcessFunction(object sender, FunctionEventArgs e)
        {
            switch (e.Name)
            {
                case "add":
                    if (e.Parameters.Count == 2)
                        e.Result = e.Parameters[0] + e.Parameters[1];
                    else
                        e.Status = FunctionStatus.WrongParameterCount;
                    break;
                case "multiply":
                    if (e.Parameters.Count == 2)
                        e.Result = e.Parameters[0] * e.Parameters[1];
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