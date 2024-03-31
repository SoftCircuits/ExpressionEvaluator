// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

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

    /// <summary>
    /// Represents the argument object passed to ProcessSymbol event.
    /// </summary>
    public class SymbolEventArgs : EventArgs
    {
        /// <summary>
        /// The name of this symbol.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the resulting value.
        /// </summary>
        public Variable Result { get; set; }

        /// <summary>
        /// Returns the resulting status.
        /// </summary>
        public SymbolStatus Status { get; set; }

        /// <summary>
        /// Creates a new <see cref="SymbolEventArgs"/> instance.
        /// </summary>
        /// <param name="name">The symbol name.</param>
        /// <param name="result">Returns the resulting value.</param>
        /// <param name="status">Returns the resulting status.</param>
        public SymbolEventArgs(string name, Variable result, SymbolStatus status = SymbolStatus.OK)
        {
            Name = name;
            Result = result;
            Status = status;
        }
    }
}
