// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperandToken(Variable value) : IToken
    {
        public Variable Value { get; private set; } = value;

        public TokenType Type => TokenType.Operand;
        public int Precedence => 0;
    }
}
