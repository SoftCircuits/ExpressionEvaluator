// Copyright (c) 2019-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Collections.Generic;

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperatorInfo
    {
        public char Operator { get; set; }
        public int Precedence { get; set; }
        public Action<Stack<Variable>> Evaluator { get; set; }

        public OperatorInfo(char @operator, int precedence, Action<Stack<Variable>> evaluator)
        {
            Operator = @operator;
            Precedence = precedence;
            Evaluator = evaluator;
        }
    }
}
