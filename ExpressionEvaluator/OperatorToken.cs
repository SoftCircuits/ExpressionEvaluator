// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperatorInfo
    {
        public char Operator { get; set; }
        public int Precedence { get; set; }
        public Action<Stack<double>> Evaluator { get; set; }

        public OperatorInfo(char @operator, int precedence, Action<Stack<double>> evaluator)
        {
            Operator = @operator;
            Precedence = precedence;
            Evaluator = evaluator;
        }
    }

    internal class OperatorToken : Token
    {

        #region Static Declarations

        public const char OpAdd = '+';
        public const char OpSubtract = '-';
        public const char OpMultiply = '*';
        public const char OpDivide = '/';
        public const char OpRemainder = '%';
        // Use otherwise unused character to represent unary minus
        public const char OpNegate = '\uffff';

        public static readonly Dictionary<char, OperatorInfo> OperatorLookup = new Dictionary<char, OperatorInfo>
        {
            [OpAdd] = new OperatorInfo(OpAdd, 1, EvalAdd),
            [OpSubtract] = new OperatorInfo(OpSubtract, 1, EvalSubtract),
            [OpMultiply] = new OperatorInfo(OpMultiply, 2, EvalMultiply),
            [OpDivide] = new OperatorInfo(OpDivide, 2, EvalDivide),
            [OpRemainder] = new OperatorInfo(OpRemainder, 2, EvalRemainder),
            [OpNegate] = new OperatorInfo(OpNegate, 10, EvalNegate),
        };

        /// <summary>
        /// Gets the <see cref="OperatorInfo"></see> that correlates with the specified operator.
        /// Returns <c>true</c> if successful, or <c>false</c> if the operator is not a recognized
        /// operator.
        /// </summary>
        /// <param name="operator">The operator character.</param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> if the operator is not a
        /// recognized operator.</returns>
        public static bool GetOperatorInfo(char @operator, out OperatorInfo info) => OperatorLookup.TryGetValue(@operator, out info);

        #endregion

        private OperatorInfo Info;

        public override TokenType Type => TokenType.Operator;

        public override int Precedence => Info.Precedence;
        public char Operator => Info.Operator;
        public Action<Stack<double>> Evaluator => Info.Evaluator;

        /// <summary>
        /// Constructs a new <see cref="OperatorToken"></see> instance.
        /// </summary>
        /// <param name="operator">The operator character.</param>
        public OperatorToken(char @operator)
        {
            if (OperatorLookup.TryGetValue(@operator, out OperatorInfo info))
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

        private static void EvalAdd(Stack<double> stack)
        {
            Debug.Assert(stack.Count >= 2);
            double value2 = stack.Pop();
            double value1 = stack.Pop();
            stack.Push(value1 + value2);
        }

        private static void EvalSubtract(Stack<double> stack)
        {
            Debug.Assert(stack.Count >= 2);
            double value2 = stack.Pop();
            double value1 = stack.Pop();
            stack.Push(value1 - value2);
        }

        private static void EvalMultiply(Stack<double> stack)
        {
            Debug.Assert(stack.Count >= 2);
            double value2 = stack.Pop();
            double value1 = stack.Pop();
            stack.Push(value1 * value2);
        }

        private static void EvalDivide(Stack<double> stack)
        {
            Debug.Assert(stack.Count >= 2);
            double value2 = stack.Pop();
            double value1 = stack.Pop();
            stack.Push(value1 / value2);
        }

        private static void EvalRemainder(Stack<double> stack)
        {
            Debug.Assert(stack.Count >= 2);
            double value2 = stack.Pop();
            double value1 = stack.Pop();
            stack.Push(value1 % value2);
        }

        private static void EvalNegate(Stack<double> stack)
        {
            Debug.Assert(stack.Count >= 1);
            stack.Push(-stack.Pop());
        }

        #endregion

    }
}
