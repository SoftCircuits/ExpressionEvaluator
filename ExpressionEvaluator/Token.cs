// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal enum TokenType
    {
        Operand,
        Operator,
        LeftParenthesis,
        RightParenthesis,
    }

    internal abstract class Token
    {
        public abstract TokenType Type { get; }
        public abstract int Precedence { get; }
    }
}
