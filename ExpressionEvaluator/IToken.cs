﻿// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    internal interface IToken
    {
        TokenType Type { get; }
        int Precedence { get; }
    }
}
