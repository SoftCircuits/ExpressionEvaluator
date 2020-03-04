# Expression Evaluator

[![NuGet version (SoftCircuits.ExpressionEvaluator)](https://img.shields.io/nuget/v/SoftCircuits.ExpressionEvaluator.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.ExpressionEvaluator/)

```
Install-Package SoftCircuits.ExpressionEvaluator
```

## Overview

ExpressionEvaluator is a .NET library that will evaluate a string expression. Supports custom functions and symbols. Expressions operands can include integers, doubles or strings. Operators can include `+`, `-`, `*`, `/`, `%` or `&`. The library provides easy integration with any application.

Custom functions and symbols are implemented using the `EvaluateFunction` and `EvaluateSymbol` events. These events are raised when ExpressionEvaluator encounters a function or symbol in the expression.

## Basic Example

This example evaluates simple expressions.

```cs
ExpressionEvaluator eval = new ExpressionEvaluator();
Variable v;

v = eval.Evaluate("2 + 2"));        // Returns 4
v = eval.Evaluate("2 + 3 * 5"));    // Returns 17
v = eval.Evaluate("(2 + 3) * 5"));  // Returns 25
```

Note that `Variable` data type. This type can hold integer, double or string data. `Variable` objects have a number of methods to retrieve and set values.

The library also supports expressions that contain strings.

```cs
ExpressionEvaluator eval = new ExpressionEvaluator();
Variable v;

v = eval.Evaluate("\"2\" & \"2\""));  // Returns "22"
v = eval.Evaluate("\"2\" + \"2\""));  // Returns "4"
```

## Expression Symbols

This example evaluates an expression with symbols. The `EvaluateSymbol` event handler defines three symbols, and sets the status to `SymbolStatus.UndefinedSymbol` if the symbol is not defined. Setting the status to `SymbolStatus.UndefinedSymbol` causes an `EvaluationException` exception to be thrown.

```cs
void Test()
{
    ExpressionEvaluator eval = new ExpressionEvaluator();
    eval.EvaluateSymbol += Eval_EvaluateSymbol;
    Variable v;

    v = eval.Evaluate("two + two")); // Returns 4
    v = eval.Evaluate("two + three * five"));   // Returns 17
    v = eval.Evaluate("(two + three) * five")); // Returns 25
}

private void Eval_EvaluateSymbol(object sender, SymbolEventArgs e)
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
## Expression Functions

The next examples employs both symbols and functions. Note that the `EvaluateFunction` event handler defines two functions. It sets the status to `FunctionStatus.UndefinedFunction` if the function is not defined. In addition, it sets the status to `FunctionStatus.WrongParameterCount` if the number of arguments passed is not valid for the function. Setting the status to `FunctionStatus.UndefinedFunction` or `FunctionStatus.WrongParameterCount` causes an `EvaluationException` exception to be thrown.

```cs
void Test()
{
    ExpressionEvaluator eval = new ExpressionEvaluator();
    eval.EvaluateSymbol += Eval_EvaluateSymbol;
    eval.EvaluateFunction += Eval_EvaluateFunction;
    Variable v;

    v = eval.Evaluate("add(two, two)"));                    // Returns 4
    v = eval.Evaluate("two + multiply(three, five)"));      // Returns 17
    v = eval.Evaluate("multiply(add(two, three), five)"));  // Returns 25
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

private void Eval_ProcessFunction(object sender, FunctionEventArgs e)
{
    switch (e.Name)
    {
        case "add":
            if (e.Parameters.Length == 2)
            {
                e.Result.SetValue(e.Parameters[0]);
                e.Result.Add(e.Parameters[1]);
            }
            else e.Status = FunctionStatus.WrongParameterCount;
            break;
        case "multiply":
            if (e.Parameters.Length == 2)
            {
                e.Result.SetValue(e.Parameters[0]);
                e.Result.Multiply(e.Parameters[1]);
            }
            else e.Status = FunctionStatus.WrongParameterCount;
            break;
        default:
            e.Status = FunctionStatus.UndefinedFunction;
            break;
    }
}
```

## Additional Information

This code was derived from the article [A C# Expression Evaluator](http://www.blackbeltcoder.com/Articles/algorithms/a-c-expression-evaluator).
