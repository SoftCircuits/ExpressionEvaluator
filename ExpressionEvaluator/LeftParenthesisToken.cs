// Copyright (c) 2023-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.ExpressionEvaluator.Enums;

namespace SoftCircuits.ExpressionEvaluator
{
    internal class LeftParenthesisToken : IToken
    {
        public TokenType Type => TokenType.LeftParenthesis;
        public int Precedence => 0;
    }
}
