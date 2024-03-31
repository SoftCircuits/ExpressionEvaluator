// Copyright (c) 2019-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// Exception thrown when an error is encountered within the expression.
    /// </summary>
    public class ExpressionException : Exception
    {
        /// <summary>
        /// Zero-base position in expression where exception occurred
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Constructs an ExpressionException instance.
        /// </summary>
        /// <param name="message">Message that describes this exception</param>
        /// <param name="index">Position within expression where exception occurred</param>
        public ExpressionException(string message, int index)
            : base(message)
        {
            Index = index;
        }

        /// <summary>
        /// Constructs an <see cref="ExpressionException"/> instance.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument"></param>
        /// <param name="index"></param>
        public ExpressionException(string format, object argument, int index)
            : base(string.Format(format, argument))
        {
            Index = index;
        }

        /// <summary>
        /// Gets the message associated with this exception
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("{0} (column {1})", base.Message, Index + 1);
            }
        }
    }
}
