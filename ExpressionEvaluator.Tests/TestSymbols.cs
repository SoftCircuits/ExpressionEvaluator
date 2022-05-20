using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.IsTrue(eval.Evaluate("two + two") == 4);
            Assert.IsTrue(eval.Evaluate("TWO + THREE * FIVE") == 17);
            Assert.IsTrue(eval.Evaluate("(two + three) * -five") == -25);
        }

        [TestMethod]
        public void TestFunctions()
        {
            ExpressionEvaluator eval = new();
            eval.EvaluateFunction += Eval_EvaluateFunction;
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            Assert.IsTrue(eval.Evaluate("add(two, two)") == 4);
            Assert.IsTrue(eval.Evaluate("TWO + multiply(THREE, FIVE)") == 17);
            Assert.IsTrue(eval.Evaluate("-multiply(add(two, three), five)") == -25);
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
