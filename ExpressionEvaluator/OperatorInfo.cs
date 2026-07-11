// Copyright (c) 2023-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Collections.Generic;

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperatorInfo(char @operator, int precedence, Action<Stack<Variable>> evaluator)
    {
        public char Operator { get; set; } = @operator;
        public int Precedence { get; set; } = precedence;
        public Action<Stack<Variable>> Evaluator { get; set; } = evaluator;
    }
}
