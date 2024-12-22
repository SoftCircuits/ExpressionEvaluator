// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

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
}
