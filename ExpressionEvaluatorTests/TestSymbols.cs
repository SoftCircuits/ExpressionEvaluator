// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.ExpressionEvaluator;

namespace ExpressionEvaluatorTests
{
    [TestClass]
    public class TestSymbols
    {
        [TestMethod]
        public void Tests()
        {
            ExpressionEvaluator eval = new();
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.AreEqual(4, eval.Evaluate("two + two"));
            Assert.AreEqual(17, eval.Evaluate("TWO + THREE * FIVE"));
            Assert.AreEqual(-25, eval.Evaluate("(two + three) * -five"));
        }

        [TestMethod]
        public void TestFunctions()
        {
            ExpressionEvaluator eval = new();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.AreEqual(4, eval.Evaluate("add(two, two)"));
            Assert.AreEqual(17, eval.Evaluate("TWO + multiply(THREE, FIVE)"));
            Assert.AreEqual(-25, eval.Evaluate("-multiply(add(two, three), five)"));
        }

        private void Eval_EvaluateSymbol(object? sender, SymbolEventArgs e)
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
