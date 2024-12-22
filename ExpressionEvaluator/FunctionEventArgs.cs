// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// Represents the argument object passed to the ProcessFunction event.
    /// </summary>
    /// <remarks>
    /// Constructs a new <see cref="FunctionEventArgs"/> instance.
    /// </remarks>
    /// <param name="name">Name of the function.</param>
    /// <param name="parameters">Parameters being passed to the function.</param>
    /// <param name="result">Returns the function result.</param>
    /// <param name="status">Returns the function status.</param>
    public class FunctionEventArgs(string name, Variable[] parameters, Variable result, FunctionStatus status = FunctionStatus.OK) : EventArgs
    {
        /// <summary>
        /// The name of this function.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Parameters passed to this function.
        /// </summary>
        public Variable[] Parameters { get; set; } = parameters;

        /// <summary>
        /// Returns the resulting value.
        /// </summary>
        public Variable Result { get; set; } = result;

        /// <summary>
        /// Returns the resulting status.
        /// </summary>
        public FunctionStatus Status { get; set; } = status;
    }
}
