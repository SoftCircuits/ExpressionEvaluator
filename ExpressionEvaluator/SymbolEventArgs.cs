// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
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

    // ProcessSymbol arguments
    public class SymbolEventArgs : EventArgs
    {
        public string Name { get; set; }
        public double Result { get; set; }
        public SymbolStatus Status { get; set; }
    }
}
