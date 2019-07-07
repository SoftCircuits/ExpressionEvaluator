# Expression Evaluator

[![NuGet version (SoftCircuits.ExpressionEvaluator)](https://img.shields.io/nuget/v/SoftCircuits.ExpressionEvaluator.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.ExpressionEvaluator/)

```
Install-Package SoftCircuits.ExpressionEvaluator
```

## Overview

Expression evaluator will evaluate a string that contains an expression and return the result of that expression. Expressions can include parentheses to control evaluation priorities and the currently supported operators are `+`, `-`, `*`, and `/`.

In addition, expressions can contain symbols and functions, and functions arguments can be expressions that also include symbols and functions. When the evaluator encounters a symbol or function, it will raise the `ProcessSymbol` or `ProcessFunction` events.

## Examples

This example evaluates simple expressions.

```cs
double d;
ExpressionEvaluator eval = new ExpressionEvaluator();

d = eval.Evaluate("2 + 2"));        // Returns 4
d = eval.Evaluate("2 + 3 * 5"));    // Returns 17
d = eval.Evaluate("(2 + 3) * 5"));  // Returns 25
```

The next example evaluates an expression with symbols. The `ProcessSymbol` event handler defines three symbols, and sets the status to `SymbolStatus.UndefinedSymbol` if the symbol is not supported. Setting the status to `SymbolStatus.UndefinedSymbol` causes an `EvaluationException` exception to be thrown.

```cs
void Test()
{
    double d;
    ExpressionEvaluator eval = new ExpressionEvaluator();
    eval.ProcessSymbol += Eval_ProcessSymbol;

    d = eval.Evaluate("two + two")); // Returns 4
    d = eval.Evaluate("two + three * five"));   // Returns 17
    d = eval.Evaluate("(two + three) * five")); // Returns 25
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
```

The next examples employs both symbols and functions. Note that the `ProcessFunction` event handler defines two functions. It sets the status to `FunctionStatus.UndefinedFunction` if the function is not supported. In addition, it sets the status to `FunctionStatus.WrongParameterCount` if the number of arguments passed is not valid for the function. Setting the status to `FunctionStatus.UndefinedFunction` or `FunctionStatus.WrongParameterCount` causes an `EvaluationException` exception to be thrown.

```cs
void Test()
{
    double d;
    ExpressionEvaluator eval = new ExpressionEvaluator();
    eval.ProcessFunction += Eval_ProcessFunction;
    eval.ProcessSymbol += Eval_ProcessSymbol;

    d = eval.Evaluate("add(two, two)"));                    // Returns 4
    d = eval.Evaluate("two + multiply(three, five)"));      // Returns 17
    d = eval.Evaluate("multiply(add(two, three), five)"));  // Returns 25
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
```

## Additional Information

For additional information, check the [A C# Expression Evaluator](http://www.blackbeltcoder.com/Articles/algorithms/a-c-expression-evaluator) article on Black Belt Coder.
