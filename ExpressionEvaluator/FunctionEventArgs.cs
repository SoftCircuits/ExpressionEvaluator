// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// Specifies the status of evaluating an expression function.
    /// </summary>
    public enum FunctionStatus
    {
        /// <summary>
        /// The function was successfully evaluated.
        /// </summary>
        OK,
        /// <summary>
        /// The function name is not supported.
        /// </summary>
        UndefinedFunction,
        /// <summary>
        /// The number of parameters does not match the function being called.
        /// </summary>
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

        /// <summary>
        /// Constructs a new <see cref="FunctionEventArgs"/> instance.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="parameters">Parameters being passed to the function.</param>
        /// <param name="result">Returns the function result.</param>
        /// <param name="status">Returns the function status.</param>
        public FunctionEventArgs(string name, Variable[] parameters, Variable result, FunctionStatus status = FunctionStatus.OK)
        {
            Name = name;
            Parameters = parameters;
            Result = result;
            Status = status;
        }
    }
}
