﻿// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal class LeftParenthesisToken : IToken
    {
        public TokenType Type => TokenType.LeftParenthesis;
        public int Precedence => 0;
    }
}
