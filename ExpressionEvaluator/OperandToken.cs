// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal class OperandToken : Token
    {
        public double Value { get; private set; }

        public override TokenType Type => TokenType.Operand;
        public override int Precedence => 0;

        public OperandToken(double value)
        {
            Value = value;
        }
    }
}
