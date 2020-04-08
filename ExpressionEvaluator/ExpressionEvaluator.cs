// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsing.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// Expression evaluator class.
    /// </summary>
    public class ExpressionEvaluator
    {
        // Token state enums
        protected enum State
        {
            None = 0,
            Operand = 1,
            Operator = 2,
            UnaryOperator = 3
        }

        // Error messages
        internal const string ErrInvalidOperand = "Invalid operand";
        internal const string ErrOperandExpected = "Operand expected";
        internal const string ErrOperatorExpected = "Operator expected";
        internal const string ErrUnmatchedClosingParen = "Closing parenthesis without matching open parenthesis";
        internal const string ErrMultipleDecimalPoints = "Operand contains multiple decimal points";
        internal const string ErrUnexpectedCharacter = "Unexpected character encountered '{0}'";
        internal const string ErrUndefinedSymbol = "Undefined symbol '{0}'";
        internal const string ErrUndefinedFunction = "Undefined function '{0}'";
        internal const string ErrClosingParenExpected = "Closing parenthesis expected";
        internal const string ErrWrongParamCount = "Wrong number of function parameters";

        // Event handers
        public event EventHandler<SymbolEventArgs> EvaluateSymbol;
        public event EventHandler<FunctionEventArgs> EvaluateFunction;

        private bool IsNumberChar(char c) => char.IsDigit(c) || c == '.';
        private bool IsSymbolFirstChar(char c) => char.IsLetter(c) || c == '_';
        private bool IsSymbolChar(char c) => char.IsLetterOrDigit(c) || c == '_';

        //
        public ExpressionEvaluator()
        {
        }

        /// <summary>
        /// Evaluates the given expression and returns the result.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>Returns the numeric result of the expression.</returns>
        public Variable Evaluate(string expression)
        {
            return EvaluateTokens(TokenizeExpression(expression));
        }

        /// <summary>
        /// Converts a standard infix expression to list of tokens in
        /// postfix order.
        /// </summary>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns>List of tokens in postfix order.</returns>
        private List<IToken> TokenizeExpression(string expression)
        {
            ParsingHelper parser = new ParsingHelper(expression);
            List<IToken> tokens = new List<IToken>();
            Stack<IToken> stack = new Stack<IToken>();
            State state = State.None;
            int parenCount = 0;
            IToken token;

            while (true)
            {
                parser.SkipWhiteSpace();
                if (parser.EndOfText)
                    break;

                // Get next character
                char c = parser.Peek();
                if (c == '(')
                {
                    // Cannot follow operand
                    if (state == State.Operand)
                        throw new ExpressionException(ErrOperatorExpected, parser.Index);
                    // Allow additional unary operators after "("
                    if (state == State.UnaryOperator)
                        state = State.Operator;
                    // Push opening parenthesis onto stack
                    stack.Push(new LeftParenthesisToken());
                    // Track number of parentheses
                    parenCount++;
                }
                else if (c == ')')
                {
                    // Must follow operand
                    if (state != State.Operand)
                        throw new ExpressionException(ErrOperandExpected, parser.Index);
                    // Must have matching open parenthesis
                    if (parenCount == 0)
                        throw new ExpressionException(ErrUnmatchedClosingParen, parser.Index);
                    // Pop all operators until matching "(" found
                    token = stack.Pop();
                    while (token.Type != TokenType.LeftParenthesis)
                    {
                        tokens.Add(token);
                        token = stack.Pop();
                    }
                    // Track number of parentheses
                    parenCount--;
                }
                else if (OperatorToken.GetOperatorInfo(c, out OperatorInfo info))
                {
                    // Need a bit of extra code to support unary operators
                    if (state == State.Operand)
                    {
                        // Pop operators with precedence >= current operator
                        while (stack.Count > 0 && stack.Peek().Precedence >= info.Precedence)
                            tokens.Add(stack.Pop());
                        stack.Push(new OperatorToken(info));
                        state = State.Operator;
                    }
                    else if (state == State.UnaryOperator)
                    {
                        // Don't allow two unary operators together
                        throw new ExpressionException(ErrOperandExpected, parser.Index);
                    }
                    else
                    {
                        // Test for unary operator
                        if (c == '-')
                        {
                            // Push unary minus
                            stack.Push(new OperatorToken(OperatorToken.OpNegate));
                            state = State.UnaryOperator;
                        }
                        else if (c == '+')
                        {
                            // Just ignore unary plus
                            state = State.UnaryOperator;
                        }
                        else
                        {
                            throw new ExpressionException(ErrOperandExpected, parser.Index);
                        }
                    }
                }
                else if (IsNumberChar(c))
                {
                    if (state == State.Operand)
                    {
                        // Cannot follow other operand
                        throw new ExpressionException(ErrOperatorExpected, parser.Index);
                    }
                    // Parse number
                    tokens.Add(new OperandToken(ParseNumber(parser)));
                    state = State.Operand;
                    continue;
                }
                else if (IsSymbolFirstChar(c))
                {
                    // Parse symbols and functions
                    if (state == State.Operand)
                        // Symbol or function cannot follow other operand
                        throw new ExpressionException(ErrOperatorExpected, parser.Index);

                    // Save start of symbol for error reporting
                    int symbolPos = parser.Index;

                    // Parse this symbol
                    string symbol = parser.ParseWhile(IsSymbolChar);

                    // Skip whitespace
                    parser.SkipWhiteSpace();
                    // Check for parameter list
                    Variable result;
                    if (parser.Peek() == '(')
                    {
                        // Found parameter list, evaluate function
                        result = ParseFunction(parser, symbol, symbolPos);
                    }
                    else
                    {
                        // No parameter list, evaluate symbol (variable)
                        result = ParseSymbol(symbol, symbolPos);
                    }
                    tokens.Add(new OperandToken(result));
                    state = State.Operand;
                    // Don't MoveAhead again
                    continue;
                }
                else if (c == '"' || c == '\'')
                {
                    // String literal
                    string text = parser.ParseQuotedText();
                    tokens.Add(new OperandToken(new Variable(text)));
                    state = State.Operand;
                }
                else
                {
                    // Unrecognized character
                    throw new ExpressionException(ErrUnexpectedCharacter, c, parser.Index);
                }
                parser++;
            }
            // Expression cannot end with operator
            if (state == State.Operator || state == State.UnaryOperator)
                throw new ExpressionException(ErrOperandExpected, parser.Index);
            // Check for balanced parentheses
            if (parenCount > 0)
                throw new ExpressionException(ErrClosingParenExpected, parser.Index);
            // Retrieve remaining operators from stack
            while (stack.Count > 0)
                tokens.Add(stack.Pop());
            return tokens;
        }

        /// <summary>
        /// Parses and extracts a numeric value at the current position.
        /// </summary>
        /// <param name="parser">Current parsing helper object.</param>
        /// <returns>The extracted number token string.</returns>
        private Variable ParseNumber(ParsingHelper parser)
        {
            Debug.Assert(IsNumberChar(parser.Peek()));

            bool hasDecimal = false;
            int start = parser.Index;
            while (char.IsDigit(parser.Peek()) || parser.Peek() == '.')
            {
                if (parser.Peek() == '.')
                {
                    if (hasDecimal)
                        throw new ExpressionException(ErrMultipleDecimalPoints, parser.Index);
                    hasDecimal = true;
                }
                parser++;
            }
            // Extract token
            string token = parser.Extract(start, parser.Index);
            if (token == ".")
                throw new ExpressionException(ErrInvalidOperand, parser.Index - 1);

            if (hasDecimal)
                return new Variable(double.Parse(token));
            return new Variable(int.Parse(token));
        }

        /// <summary>
        /// This method evaluates a symbol name and returns its value.
        /// </summary>
        /// <param name="name">Name of symbol.</param>
        /// <param name="pos">Position at start of symbol.</param>
        /// <returns></returns>
        protected Variable ParseSymbol(string name, int pos)
        {
            // Create event args
            SymbolEventArgs args = new SymbolEventArgs
            {
                Name = name,
                Result = new Variable(),
                Status = SymbolStatus.OK,
            };

            if (EvaluateSymbol != null)
                EvaluateSymbol(this, args);
            else
                args.Status = SymbolStatus.UndefinedSymbol;

            // Throw exception if error
            if (args.Status == SymbolStatus.UndefinedSymbol)
                throw new ExpressionException(string.Format(ErrUndefinedSymbol, name), pos);
            return args.Result;
        }

        /// <summary>
        /// Evaluates a function and returns its value. It is assumed the current
        /// position is at the opening parenthesis of the argument list.
        /// </summary>
        /// <param name="parser">ParsingHelper object.</param>
        /// <param name="name">Name of function.</param>
        /// <param name="pos">Position at start of function.</param>
        /// <returns></returns>
        private Variable ParseFunction(ParsingHelper parser, string name, int pos)
        {
            FunctionEventArgs args = new FunctionEventArgs
            {
                Name = name,
                Parameters = ParseParameters(parser),
                Result = new Variable(),
                Status = FunctionStatus.OK,
            };

            if (EvaluateFunction != null)
                EvaluateFunction(this, args);
            else
                args.Status = FunctionStatus.UndefinedFunction;

            if (args.Status == FunctionStatus.UndefinedFunction)
                throw new ExpressionException(string.Format(ErrUndefinedFunction, name), pos);
            if (args.Status == FunctionStatus.WrongParameterCount)
                throw new ExpressionException(ErrWrongParamCount, pos);
            return args.Result;
        }

        /// <summary>
        /// Evaluates each parameter of a function's parameter list and returns
        /// a list of those values. An empty list is returned if no parameters
        /// were found. It is assumed the current position is at the opening
        /// parenthesis of the argument list.
        /// </summary>
        /// <param name="parser">ParsingHelper object.</param>
        /// <returns>A list of parameter values.</returns>
        private Variable[] ParseParameters(ParsingHelper parser)
        {
            List<Variable> parameters = new List<Variable>();

            // Move past open parenthesis
            parser++;
            parser.SkipWhiteSpace();
            // If function has any parameters
            if (parser.Peek() != ')')
            {
                // Parse function parameter list
                int start = parser.Index;
                int parenCount = 1;

                while (!parser.EndOfText)
                {
                    if (parser.Peek() == ',')
                    {
                        // Note: Ignore commas inside parentheses. They could be
                        // for a parameter list in a function inside the parameters
                        if (parenCount == 1)
                        {
                            parameters.Add(ParseParameter(parser, start));
                            start = parser.Index + 1;
                        }
                    }
                    if (parser.Peek() == ')')
                    {
                        parenCount--;
                        if (parenCount == 0)
                        {
                            parameters.Add(ParseParameter(parser, start));
                            break;
                        }
                    }
                    else if (parser.Peek() == '(')
                    {
                        parenCount++;
                    }
                    parser++;
                }
            }
            // Make sure we found a closing parenthesis
            if (parser.Peek() != ')')
                throw new ExpressionException(ErrClosingParenExpected, parser.Index);
            // Move past closing parenthesis
            parser++;
            // Return parameter list
            return parameters.ToArray();
        }

        /// <summary>
        /// Extracts and evaluates a function parameter and returns its value. If an
        /// exception occurs, it is caught and the column is adjusted to reflect the
        /// position in original string, and the exception is rethrown.
        /// </summary>
        /// <param name="parser">ParsingHelper object.</param>
        /// <param name="start">Index where current parameter starts.</param>
        /// <returns>The parameter value.</returns>
        private Variable ParseParameter(ParsingHelper parser, int start)
        {
            try
            {
                // Recursively evaluate expression
                string expression = parser.Extract(start, parser.Index);
                return Evaluate(expression);
            }
            catch (ExpressionException ex)
            {
                // Adjust column and rethrow exception
                ex.Index += start;
                throw ex;
            }
        }

        /// <summary>
        /// Evaluates the given list of tokens and returns the result.
        /// Tokens must appear in postfix order.
        /// </summary>
        /// <param name="tokens">List of tokens to evaluate.</param>
        /// <returns>The result of all tokens.</returns>
        private Variable EvaluateTokens(List<IToken> tokens)
        {
            Stack<Variable> stack = new Stack<Variable>();

            Debug.Assert(tokens.All(t => t.Type == TokenType.Operand || t.Type == TokenType.Operator));
            foreach (IToken token in tokens)
            {
                if (token is OperandToken operandToken)
                    stack.Push(operandToken.Value);
                else if (token is OperatorToken operatorToken)
                    operatorToken.Evaluator(stack);
            }
            // Remaining item on stack contains result
            Debug.Assert(stack.Count == 1);
            return (stack.Count > 0) ? stack.Pop() : new Variable();
        }
    }
}
