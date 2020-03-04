// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.ExpressionEvaluator
{
    public enum SymbolStatus
    {
        OK,
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
    }
}
