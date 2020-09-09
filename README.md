<a href="https://www.buymeacoffee.com/jonathanwood" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/black_img.png" alt="Buy Me A Coffee" style="height: 37px !important;width: 170px !important;" ></a>

# Expression Evaluator

[![NuGet version (SoftCircuits.ExpressionEvaluator)](https://img.shields.io/nuget/v/SoftCircuits.ExpressionEvaluator.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.ExpressionEvaluator/)

```
Install-Package SoftCircuits.ExpressionEvaluator
```

## Overview

ExpressionEvaluator is a .NET library that will evaluate a string expression. It supports custom functions and symbols. Expression operands can include integers, doubles and strings. And operators include `+`, `-`, `*`, `/`, `%` (modulus), `^` (power) and `&` (concatenation).

The library easily integrates with any .NET application. Custom functions and symbols are implemented using the `EvaluateFunction` and `EvaluateSymbol` events. These events are raised when ExpressionEvaluator encounters a function or symbol in the expression.

## Basic Example

Use the `Evaluate()` method to evaluate a string expression and return the result.

```cs
ExpressionEvaluator eval = new ExpressionEvaluator();
Variable v;

v = eval.Evaluate("2 + 2");        // Returns 4  (Integer)
v = eval.Evaluate("2 + 3 * 5");    // Returns 17 (Integer)
v = eval.Evaluate("(2 + 3) * 5");  // Returns 25 (Integer)
```

As you can see in the example above, the `Evaluate()` method returns a `Variable`. A `Variable` can hold an integer, double or string value. You can use the `Type` property to determine its current type, or just call the `ToString()` method to convert the value to a string. The `Variable` class includes methods to set its value, convert its value to another type and perform various operations. It also overloads many operators to make it easier to work with.

Expressions may also include string literals. Strings are any text enclosed in either double or single quotes. If the string contains two quotes together, they will be interpreted as a single quote character rather than the end of the string.

```cs
ExpressionEvaluator eval = new ExpressionEvaluator();
Variable v;

v = eval.Evaluate("\"2\" & \"2\"");  // Returns 22 (String)
v = eval.Evaluate("'2' & '2'");      // Returns 22 (String)
v = eval.Evaluate("\"2\" + \"2\"");  // Returns 4  (Integer)
```

Note the concatenation operator (`&`). This operator converts both operands to a string (if needed) and then concatenates them.

## Expression Symbols

This example evaluates an expression with symbols. The `EvaluateSymbol` event handler defines three symbols (`"two"`, `"three"`, and `"five"`), and sets the status to `SymbolStatus.UndefinedSymbol` if the event is raised for an unknown symbol. Setting the status to `SymbolStatus.UndefinedSymbol` causes an `EvaluationException` exception to be thrown.

```cs
public void Test()
{
    ExpressionEvaluator eval = new ExpressionEvaluator();
    eval.EvaluateSymbol += Eval_EvaluateSymbol;
    Variable v;

    v = eval.Evaluate("two + two");            // Returns 4  (Integer)
    v = eval.Evaluate("two + three * five");   // Returns 17 (Integer)
    v = eval.Evaluate("(two + three) * five"); // Returns 25 (Integer)
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
```

## Expression Functions

The next example defines two custom functions (`"add"` and `"multiply"`). The `EvaluateFunction` event handler sets the status to `FunctionStatus.UndefinedFunction` if the function name is not supported. In addition, it sets the status to `FunctionStatus.WrongParameterCount` if the number of arguments passed is not valid for the function. Setting the status to `FunctionStatus.UndefinedFunction` or `FunctionStatus.WrongParameterCount` causes an `EvaluationException` exception to be thrown.

```cs
void Test()
{
    ExpressionEvaluator eval = new ExpressionEvaluator();
    eval.EvaluateFunction += Eval_EvaluateFunction;
    Variable v;

    v = eval.Evaluate("add(2, 2)");               // Returns 4  (Integer)
    v = eval.Evaluate("2 + multiply(3, 5)");      // Returns 17 (Integer)
    v = eval.Evaluate("multiply(add(2, 3), 5)");  // Returns 25 (Integer)
}

private void Eval_EvaluateFunction(object sender, FunctionEventArgs e)
{
    switch (e.Name.ToUpper())
    {
        case "ADD":
            if (e.Parameters.Length == 2)
                e.Result.SetValue(e.Parameters[0] + e.Parameters[1]);
            else
                e.Status = FunctionStatus.WrongParameterCount;
            break;
        case "MULTIPLY":
            if (e.Parameters.Length == 2)
                e.Result.SetValue(e.Parameters[0] * e.Parameters[1]);
            else
                e.Status = FunctionStatus.WrongParameterCount;
            break;
        default:
            e.Status = FunctionStatus.UndefinedFunction;
            break;
    }
}
```

As the example above demonstrates, expressions can include nested functions (functions that are passed the result of another function or expression).

## Additional Information

This code was derived from the article [A C# Expression Evaluator](http://www.blackbeltcoder.com/Articles/algorithms/a-c-expression-evaluator).
