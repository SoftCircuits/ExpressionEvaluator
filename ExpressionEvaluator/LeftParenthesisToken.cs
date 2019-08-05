// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal class LeftParenthesisToken : Token
    {
        public override TokenType Type => TokenType.LeftParenthesis;
        public override int Precedence => 0;
    }
}
