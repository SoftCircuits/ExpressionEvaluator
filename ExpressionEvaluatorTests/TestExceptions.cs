// Copyright (c) 2023-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.ExpressionEvaluator;
using SoftCircuits.ExpressionEvaluator.Enums;

namespace ExpressionEvaluatorTests
{
    [TestClass]
    public class TestExceptions
    {
        [TestMethod]
        public void TestSyntaxErrorException()
        {
            Assert.Throws<ExpressionException>(() =>
            {
                ExpressionEvaluator eval = new();
                eval.Evaluate("2 + ");
            });
        }

        [TestMethod]
        public void TestUndefinedSymbolException()
        {
            Assert.Throws<ExpressionException>(() =>
            {
                ExpressionEvaluator eval = new();
                eval.Evaluate("two + two");
            });
        }

        [TestMethod]
        public void TestUndefinedFunctionException()
        {
            Assert.Throws<ExpressionException>(() =>
            {
                ExpressionEvaluator eval = new();
                eval.Evaluate("2 + f()");
            });
        }

        [TestMethod]
        public void TestWrongParameterCountException()
        {
            Assert.Throws<ExpressionException>(() =>
            {
                ExpressionEvaluator eval = new();
                eval.EvaluateFunction += Eval_EvaluateFunction;
                eval.Evaluate("add(1, 2, 3)");
            });
        }

        private void Eval_EvaluateFunction(object? sender, FunctionEventArgs e)
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
