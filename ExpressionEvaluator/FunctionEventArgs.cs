// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.ExpressionEvaluator
{
    public enum FunctionStatus
    {
        OK,
        UndefinedFunction,
        WrongParameterCount,
    }

    /// <summary>
    /// Represents the argument object passed to the ProcessFunction event.
    /// </summary>
    public class FunctionEventArgs : EventArgs
    {
        /// <summary>
        /// The name of this function.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parameters passed to this function.
        /// </summary>
        public Variable[] Parameters { get; set; }

        /// <summary>
        /// Returns the resulting value.
        /// </summary>
        public Variable Result { get; set; }

        /// <summary>
        /// Returns the resulting status.
        /// </summary>
        public FunctionStatus Status { get; set; }
    }
}
