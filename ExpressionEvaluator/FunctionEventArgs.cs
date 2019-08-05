// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;

namespace SoftCircuits.ExpressionEvaluator
{
    public enum FunctionStatus
    {
        OK,
        UndefinedFunction,
        WrongParameterCount,
    }

    // ProcessFunction arguments
    public class FunctionEventArgs : EventArgs
    {
        public string Name { get; set; }
        public double[] Parameters { get; set; }
        public double Result { get; set; }
        public FunctionStatus Status { get; set; }
    }
}
