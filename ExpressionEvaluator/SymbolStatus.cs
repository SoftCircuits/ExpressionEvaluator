// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// Specifies the status of evaluating an expression.
    /// </summary>
    public enum SymbolStatus
    {
        /// <summary>
        /// The symbol was successfully evaluated.
        /// </summary>
        OK,
        /// <summary>
        /// The symbol name is unsupported.
        /// </summary>
        UndefinedSymbol,
    }
}
