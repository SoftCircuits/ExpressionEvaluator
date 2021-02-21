// Copyright (c) 2019-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperatorToken : IToken
    {

        #region Static Declarations

        public const char OpAdd = '+';
        public const char OpSubtract = '-';
        public const char OpMultiply = '*';
        public const char OpDivide = '/';
        public const char OpModulus = '%';
        public const char OpPower = '^';
        public const char OpConcatenate = '&';
        public const char OpNegate = '\uffff';  // Represents unary minus

        public static readonly Dictionary<char, OperatorInfo> OperatorLookup = new Dictionary<char, OperatorInfo>
        {
            [OpConcatenate] = new OperatorInfo(OpConcatenate, 1, EvalConcatenate),
            [OpAdd] = new OperatorInfo(OpAdd, 2, EvalAdd),
            [OpSubtract] = new OperatorInfo(OpSubtract, 2, EvalSubtract),
            [OpModulus] = new OperatorInfo(OpModulus, 3, EvalModulus),
            [OpMultiply] = new OperatorInfo(OpMultiply, 4, EvalMultiply),
            [OpDivide] = new OperatorInfo(OpDivide, 4, EvalDivide),
            [OpNegate] = new OperatorInfo(OpNegate, 5, EvalNegate),
            [OpPower] = new OperatorInfo(OpPower, 6, EvalPower),
        };

        /// <summary>
        /// Gets the <see cref="OperatorInfo"></see> that correlates with the specified operator.
        /// Returns <c>true</c> if successful, or <c>false</c> if the operator is not a recognized
        /// operator.
        /// </summary>
        /// <param name="operator">The operator character.</param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> if the operator is not a
        /// recognized operator.</returns>
#if NETSTANDARD2_0
        public static bool GetOperatorInfo(char @operator, out OperatorInfo info) => OperatorLookup.TryGetValue(@operator, out info);
#else
        public static bool GetOperatorInfo(char @operator, [MaybeNullWhen(false)] out OperatorInfo info) => OperatorLookup.TryGetValue(@operator, out info);
#endif

        #endregion

        private readonly OperatorInfo Info;

        public TokenType Type => TokenType.Operator;

        public int Precedence => Info.Precedence;
        public char Operator => Info.Operator;
        public Action<Stack<Variable>> Evaluator => Info.Evaluator;

        /// <summary>
        /// Constructs a new <see cref="OperatorToken"></see> instance.
        /// </summary>
        /// <param name="operator">The operator character.</param>
        public OperatorToken(char @operator)
        {
            if (OperatorLookup.TryGetValue(@operator, out OperatorInfo? info))
                Info = info;
            else
                throw new Exception($"Invalid operator token : '{@operator}'.");
        }

        /// <summary>
        /// Constructs a new <see cref="OperatorToken"></see> instance.
        /// </summary>
        /// <param name="info">The <see cref="OperatorInfo"></see> that this token
        /// represents.</param>
        public OperatorToken(OperatorInfo info)
        {
            Info = info;
        }

#region Evaluators

        private static void EvalAdd(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Add(var2);
            stack.Push(var1);
        }

        private static void EvalSubtract(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Subtract(var2);
            stack.Push(var1);
        }

        private static void EvalMultiply(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Multiply(var2);
            stack.Push(var1);
        }

        private static void EvalDivide(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Divide(var2);
            stack.Push(var1);
        }

        private static void EvalModulus(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Modulus(var2);
            stack.Push(var1);
        }

        private static void EvalPower(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Power(var2);
            stack.Push(var1);
        }

        private static void EvalConcatenate(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 2);
            Variable var2 = stack.Pop();
            Variable var1 = stack.Pop();
            var1.Concatenate(var2);
            stack.Push(var1);
        }

        private static void EvalNegate(Stack<Variable> stack)
        {
            Debug.Assert(stack.Count >= 1);
            Variable var1 = stack.Pop();
            var1.Negate();
            stack.Push(var1);
        }

#endregion

    }
}
