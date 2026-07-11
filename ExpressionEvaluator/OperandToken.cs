// Copyright (c) 2023-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.ExpressionEvaluator.Enums;

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperandToken(Variable value) : IToken
    {
        public Variable Value { get; private set; } = value;

        public TokenType Type => TokenType.Operand;
        public int Precedence => 0;
    }
}
