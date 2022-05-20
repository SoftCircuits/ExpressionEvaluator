// Copyright (c) 2019-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperandToken : IToken
    {
        public Variable Value { get; private set; }

        public TokenType Type => TokenType.Operand;
        public int Precedence => 0;

        public OperandToken(Variable value)
        {
            Value = value;
        }
    }
}
