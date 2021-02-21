// Copyright (c) 2019-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal enum TokenType
    {
        Operand,
        Operator,
        LeftParenthesis,
        RightParenthesis,   // Not used
    }

    internal interface IToken
    {
        TokenType Type { get; }
        int Precedence { get; }
    }
}
