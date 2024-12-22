// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// Represents the argument object passed to ProcessSymbol event.
    /// </summary>
    /// <remarks>
    /// Creates a new <see cref="SymbolEventArgs"/> instance.
    /// </remarks>
    /// <param name="name">The symbol name.</param>
    /// <param name="result">Returns the resulting value.</param>
    /// <param name="status">Returns the resulting status.</param>
    public class SymbolEventArgs(string name, Variable result, SymbolStatus status = SymbolStatus.OK) : EventArgs
    {
        /// <summary>
        /// The name of this symbol.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Returns the resulting value.
        /// </summary>
        public Variable Result { get; set; } = result;

        /// <summary>
        /// Returns the resulting status.
        /// </summary>
        public SymbolStatus Status { get; set; } = status;
    }
}
