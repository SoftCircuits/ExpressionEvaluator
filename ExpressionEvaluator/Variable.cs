// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Diagnostics;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// The type of value currently held by a <see cref="Variable"></see>.
    /// </summary>
    public enum VariableType
    {
        Integer,
        Double,
        String,
    };

    /// <summary>
    /// Represents a variable. Can hold an integer, floating point, or string value.
    /// </summary>
    public class Variable : IEquatable<Variable>
    {
        // Internal value
        private int IntegerValue;
        private double DoubleValue;
        private string StringValue;

        /// <summary>
        /// Returns the current data type of this variable.
        /// </summary>
        public VariableType Type { get; private set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="Variable"></see> instance. Sets the value to
        /// 0 (integer).
        /// </summary>
        public Variable()
        {
            SetValue(default(int));
        }

        /// <summary>
        /// Constructs a new <see cref="Variable"></see> instance.
        /// </summary>
        /// <param name="value">Specifies the initial value for this object.</param>
        public Variable(int value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Constructs a new <see cref="Variable"></see> instance.
        /// </summary>
        /// <param name="value">Specifies the initial value for this object.</param>
        public Variable(double value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Constructs a new <see cref="Variable"></see> instance.
        /// </summary>
        /// <param name="value">Specifies the initial value for this object.</param>
        public Variable(string value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Constructs a new <see cref="Variable"></see> instance.
        /// </summary>
        /// <param name="value">Specifies the initial value for this object.</param>
        public Variable(Variable value)
        {
            SetValue(value);
        }

        #endregion

        #region Assignment

        /// <summary>
        /// Sets this variable to the specified value.
        /// </summary>
        /// <param name="value">The value to assign to this Variable.</param>
        public void SetValue(int value)
        {
            Type = VariableType.Integer;
            IntegerValue = value;
        }

        /// <summary>
        /// Sets this variable to the specified value.
        /// </summary>
        /// <param name="value">The value to assign to this Variable.</param>
        public void SetValue(double value)
        {
            Type = VariableType.Double;
            DoubleValue = value;
        }

        /// <summary>
        /// Sets this variable to the specified value.
        /// </summary>
        /// <param name="value">The value to assign to this Variable.</param>
        public void SetValue(string value)
        {
            Type = VariableType.String;
            StringValue = value ?? string.Empty;
        }

        /// <summary>
        /// Sets this variable to the specified value.
        /// </summary>
        /// <param name="value">The value to assign to this Variable.</param>
        public void SetValue(Variable value)
        {
            Type = value.Type;
            switch (Type)
            {
                case VariableType.Integer:
                    IntegerValue = value.IntegerValue;
                    break;
                case VariableType.Double:
                    DoubleValue = value.DoubleValue;
                    break;
                case VariableType.String:
                    StringValue = value.StringValue ?? string.Empty;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the current value as an integer.
        /// </summary>
        public int ToInteger()
        {
            switch (Type)
            {
                case VariableType.Integer:
                    return IntegerValue;
                case VariableType.Double:
                    return (int)Math.Round(DoubleValue);
                case VariableType.String:
                    return double.TryParse(StringValue, out double value) ? (int)Math.Round(value) : default;
                default:
                    Debug.Assert(false);
                    return default;
            }
        }

        /// <summary>
        /// Returns the current value as a double.
        /// </summary>
        public double ToDouble()
        {
            switch (Type)
            {
                case VariableType.Integer:
                    return IntegerValue;
                case VariableType.Double:
                    return DoubleValue;
                case VariableType.String:
                    return double.TryParse(StringValue, out double value) ? value : default;
                default:
                    Debug.Assert(false);
                    return default;
            }
        }

        /// <summary>
        /// Returns the current value as a string.
        /// </summary>
        public override string ToString()
        {
            switch (Type)
            {
                case VariableType.Integer:
                    return IntegerValue.ToString();
                case VariableType.Double:
                    return DoubleValue.ToString();
                case VariableType.String:
                    return StringValue;
                default:
                    Debug.Assert(false);
                    return string.Empty;
            }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(int value) => Calculate(value, Add);

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(double value) => Calculate(value, Add);

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(string value) => Calculate(value, Add);

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(Variable value) => Calculate(value, Add);

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(int value) => Calculate(value, Subtract);

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(double value) => Calculate(value, Subtract);

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(string value) => Calculate(value, Subtract);

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(Variable value) => Calculate(value, Subtract);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(int value) => Calculate(value, Multiply);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(double value) => Calculate(value, Multiply);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(string value) => Calculate(value, Multiply);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(Variable value) => Calculate(value, Multiply);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(int value) => Calculate(value, Divide);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(double value) => Calculate(value, Divide);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(string value) => Calculate(value, Divide);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(Variable value) => Calculate(value, Divide);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Modulus(int value) => Calculate(value, Modulus);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Modulus(double value) => Calculate(value, Modulus);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Modulus(string value) => Calculate(value, Modulus);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Modulus(Variable value) => Calculate(value, Modulus);

        /// <summary>
        /// Raises this Variable to the given power.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Power(int value) => Calculate(value, Power);

        /// <summary>
        /// Raises this Variable to the given power.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Power(double value) => Calculate(value, Power);

        /// <summary>
        /// Raises this Variable to the given power.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Power(string value) => Calculate(value, Power);

        /// <summary>
        /// Raises this Variable to the given power.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Power(Variable value) => Calculate(value, Power);

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="value">The value to be concatenated.</param>
        public void Concatenate(int value) => SetValue(ToString() + value.ToString());

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="value">The value to be concatenated.</param>
        public void Concatenate(double value) => SetValue(ToString() + value.ToString());

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="value">The value to be concatenated.</param>
        public void Concatenate(string value) => SetValue(ToString() + value);

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="value">The value to be concatenated.</param>
        public void Concatenate(Variable value) => SetValue(ToString() + value.ToString());

        /// <summary>
        /// Negates the value of this Variable.
        /// </summary>
        public void Negate()
        {
            switch (Type)
            {
                case VariableType.Integer:
                    IntegerValue = -IntegerValue;
                    break;
                case VariableType.Double:
                    DoubleValue = -DoubleValue;
                    break;
                case VariableType.String:
                    switch (GetNumericValue(StringValue, out double value))
                    {
                        case NumericType.None:
                            SetValue(0);
                            break;
                        case NumericType.Integer:
                            SetValue(-(int)value);
                            break;
                        case NumericType.Double:
                            SetValue(-value);
                            break;
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        #endregion

        #region Conversion operators

        public static implicit operator int(Variable v) => v.ToInteger();
        public static implicit operator double(Variable v) => v.ToDouble();
        public static implicit operator string(Variable v) => v.ToString();

        #endregion

        #region Comparison operators

        public static bool operator ==(Variable value1, int value2) => Compare(value1, value2) == 0;
        public static bool operator ==(Variable value1, double value2) => Compare(value1, value2) == 0;
        public static bool operator ==(Variable value1, string value2) => Compare(value1, value2) == 0;
        public static bool operator ==(Variable value1, Variable value2) => Compare(value1, value2) == 0;

        public static bool operator !=(Variable value1, int value2) => Compare(value1, value2) != 0;
        public static bool operator !=(Variable value1, double value2) => Compare(value1, value2) != 0;
        public static bool operator !=(Variable value1, string value2) => Compare(value1, value2) != 0;
        public static bool operator !=(Variable value1, Variable value2) => Compare(value1, value2) != 0;

        public static bool operator <(Variable value1, int value2) => Compare(value1, value2) < 0;
        public static bool operator <(Variable value1, double value2) => Compare(value1, value2) < 0;
        public static bool operator <(Variable value1, string value2) => Compare(value1, value2) < 0;
        public static bool operator <(Variable value1, Variable value2) => Compare(value1, value2) < 0;

        public static bool operator <=(Variable value1, int value2) => Compare(value1, value2) <= 0;
        public static bool operator <=(Variable value1, double value2) => Compare(value1, value2) <= 0;
        public static bool operator <=(Variable value1, string value2) => Compare(value1, value2) <= 0;
        public static bool operator <=(Variable value1, Variable value2) => Compare(value1, value2) <= 0;

        public static bool operator >(Variable value1, int value2) => Compare(value1, value2) > 0;
        public static bool operator >(Variable value1, double value2) => Compare(value1, value2) > 0;
        public static bool operator >(Variable value1, string value2) => Compare(value1, value2) > 0;
        public static bool operator >(Variable value1, Variable value2) => Compare(value1, value2) > 0;

        public static bool operator >=(Variable value1, int value2) => Compare(value1, value2) >= 0;
        public static bool operator >=(Variable value1, double value2) => Compare(value1, value2) >= 0;
        public static bool operator >=(Variable value1, string value2) => Compare(value1, value2) >= 0;
        public static bool operator >=(Variable value1, Variable value2) => Compare(value1, value2) >= 0;

        #endregion

        #region Operation operators

        public static Variable operator +(Variable value1, int value2) => Calculate(value1, value2, Add);
        public static Variable operator +(Variable value1, double value2) => Calculate(value1, value2, Add);
        public static Variable operator +(Variable value1, string value2) => Calculate(value1, value2, Add);
        public static Variable operator +(Variable value1, Variable value2) => Calculate(value1, value2, Add);

        public static Variable operator -(Variable value1, int value2) => Calculate(value1, value2, Subtract);
        public static Variable operator -(Variable value1, double value2) => Calculate(value1, value2, Subtract);
        public static Variable operator -(Variable value1, string value2) => Calculate(value1, value2, Subtract);
        public static Variable operator -(Variable value1, Variable value2) => Calculate(value1, value2, Subtract);

        public static Variable operator *(Variable value1, int value2) => Calculate(value1, value2, Multiply);
        public static Variable operator *(Variable value1, double value2) => Calculate(value1, value2, Multiply);
        public static Variable operator *(Variable value1, string value2) => Calculate(value1, value2, Multiply);
        public static Variable operator *(Variable value1, Variable value2) => Calculate(value1, value2, Multiply);

        public static Variable operator /(Variable value1, int value2) => Calculate(value1, value2, Divide);
        public static Variable operator /(Variable value1, double value2) => Calculate(value1, value2, Divide);
        public static Variable operator /(Variable value1, string value2) => Calculate(value1, value2, Divide);
        public static Variable operator /(Variable value1, Variable value2) => Calculate(value1, value2, Divide);

        public static Variable operator %(Variable value1, int value2) => Calculate(value1, value2, Modulus);
        public static Variable operator %(Variable value1, double value2) => Calculate(value1, value2, Modulus);
        public static Variable operator %(Variable value1, string value2) => Calculate(value1, value2, Modulus);
        public static Variable operator %(Variable value1, Variable value2) => Calculate(value1, value2, Modulus);

        public static Variable operator ^(Variable value1, int value2) => Calculate(value1, value2, Power);
        public static Variable operator ^(Variable value1, double value2) => Calculate(value1, value2, Power);
        public static Variable operator ^(Variable value1, string value2) => Calculate(value1, value2, Power);
        public static Variable operator ^(Variable value1, Variable value2) => Calculate(value1, value2, Power);

        public static Variable operator &(Variable value1, int value2) => new Variable(value1.ToString() + value2.ToString());
        public static Variable operator &(Variable value1, double value2) => new Variable(value1.ToString() + value2.ToString());
        public static Variable operator &(Variable value1, string value2) => new Variable(value1.ToString() + value2);
        public static Variable operator &(Variable value1, Variable value2) => new Variable(value1.ToString() + value2.ToString());

        public static Variable operator -(Variable value)
        {
            Variable v = new Variable(value);
            v.Negate();
            return v;
        }

        #endregion

        #region Private helpers

        // Calculation delegates

        private static double Add(double value, double operand) => value + operand;
        private static double Subtract(double value, double operand) => value - operand;
        private static double Multiply(double value, double operand) => value * operand;
        private static double Divide(double value, double operand) => (operand == 0) ? 0 : value / operand;
        private static double Modulus(double value, double operand) => (operand == 0) ? 0 : value % operand;
        private static double Power(double value, double operand) => Math.Pow(value, operand);

        /// <summary>
        /// Returns a negative value if <paramref name="value1"/> is less than <paramref name="value2"/>,
        /// a positive value if <paramref name="value1"/> is greater than <paramref name="value2"/>,
        /// or zero if they are equal.
        /// </summary>
        private static double Compare(Variable value1, int value2)
        {
            // Compare as numbers if possible
            if (value1.GetNumericValue(out double v) != NumericType.None)
                return v - value2;
            // Else compare as strings
            return string.CompareOrdinal(value1.ToString(), value2.ToString());
        }

        /// <summary>
        /// Returns a negative value if <paramref name="value1"/> is less than <paramref name="value2"/>,
        /// a positive value if <paramref name="value1"/> is greater than <paramref name="value2"/>,
        /// or zero if they are equal.
        /// </summary>
        private static double Compare(Variable value1, double value2)
        {
            // Compare as numbers if possible
            if (value1.GetNumericValue(out double v) != NumericType.None)
                return v - value2;
            // Else compare as strings
            return string.CompareOrdinal(value1.ToString(), value2.ToString());
        }

        /// <summary>
        /// Returns a negative value if <paramref name="value1"/> is less than <paramref name="value2"/>,
        /// a positive value if <paramref name="value1"/> is greater than <paramref name="value2"/>,
        /// or zero if they are equal.
        /// </summary>
        private static double Compare(Variable value1, string value2)
        {
            // Compare as numbers if possible
            if (value1.GetNumericValue(out double v1) != NumericType.None &&
                GetNumericValue(value2, out double v2) != NumericType.None)
                return v1 - v2;
            // Else compare as strings
            return string.CompareOrdinal(value1.ToString(), value2.ToString());
        }

        /// <summary>
        /// Returns a negative value if <paramref name="value1"/> is less than <paramref name="value2"/>,
        /// a positive value if <paramref name="value1"/> is greater than <paramref name="value2"/>,
        /// or zero if they are equal.
        /// </summary>
        private static double Compare(Variable value1, Variable value2)
        {
            // Compare as numbers if possible
            if (value1.GetNumericValue(out double v1) != NumericType.None &&
                value2.GetNumericValue(out double v2) != NumericType.None)
                return v1 - v2;
            // Else compare as strings
            return string.CompareOrdinal(value1.ToString(), value2);
        }

        /// <summary>
        /// Performs the given calculation against this Variable and the given operand.
        /// </summary>
        /// <param name="operand">The operand on which to perform the calculation.</param>
        /// <param name="calculator">Delegate that performs the calculation.</param>
        private void Calculate(int operand, Func<double, double, double> calculator)
        {
            // Get values
            var type = GetNumericValue(out double value);
            // Perform calculation
            value = calculator(value, operand);
            // Set result
            if (type == NumericType.Double)
                SetValue(value);
            else
                SetValue((int)value);
        }

        /// <summary>
        /// Performs the given calculation against this Variable and the given operand.
        /// </summary>
        /// <param name="operand">The operand on which to perform the calculation.</param>
        /// <param name="calculator">Delegate that performs the calculation.</param>
        private void Calculate(double operand, Func<double, double, double> calculator)
        {
            // Set value
            SetValue(calculator(ToDouble(), operand));
        }

        /// <summary>
        /// Performs the given calculation against this Variable and the given operand.
        /// </summary>
        /// <param name="operand">The operand on which to perform the calculation.</param>
        /// <param name="calculator">Delegate that performs the calculation.</param>
        private void Calculate(string operand, Func<double, double, double> calculator)
        {
            // Get values
            var valType = GetNumericValue(out double value);
            var opType = GetNumericValue(operand, out double operand2);
            // Perform calculation
            value = calculator(value, operand2);
            // Set result
            if (valType == NumericType.Double || opType == NumericType.Double)
                SetValue(value);
            else
                SetValue((int)value);
        }

        /// <summary>
        /// Performs the given calculation against this Variable and the given operand.
        /// </summary>
        /// <param name="operand">The operand on which to perform the calculation.</param>
        /// <param name="calculator">Delegate that performs the calculation.</param>
        private void Calculate(Variable operand, Func<double, double, double> calculator)
        {
            // Get values
            var valType = GetNumericValue(out double value);
            var opType = operand.GetNumericValue(out double operand2);
            // Perform calculation
            value = calculator(value, operand2);
            // Set result
            if (valType == NumericType.Double || opType == NumericType.Double)
                SetValue(value);
            else
                SetValue((int)value);
        }

        /// <summary>
        /// Static wrapper for <see cref="Calculate(int, Func{double, double, double})"></see>.
        /// Assigns results to a new <see cref="Variable"></see> and returns that variable.
        /// </summary>
        private static Variable Calculate(Variable value1, int value2, Func<double, double, double> calculator)
        {
            Variable var = new Variable(value1);
            var.Calculate(value2, calculator);
            return var;
        }

        /// <summary>
        /// Static wrapper for <see cref="Calculate(double, Func{double, double, double})"></see>.
        /// Assigns results to a new <see cref="Variable"></see> and returns that variable.
        /// </summary>
        private static Variable Calculate(Variable value1, double value2, Func<double, double, double> calculator)
        {
            Variable var = new Variable(value1);
            var.Calculate(value2, calculator);
            return var;
        }

        /// <summary>
        /// Static wrapper for <see cref="Calculate(string, Func{double, double, double})"></see>.
        /// Assigns results to a new <see cref="Variable"></see> and returns that variable.
        /// </summary>
        private static Variable Calculate(Variable value1, string value2, Func<double, double, double> calculator)
        {
            Variable var = new Variable(value1);
            var.Calculate(value2, calculator);
            return var;
        }

        /// <summary>
        /// Static wrapper for <see cref="Calculate(Variable, Func{double, double, double})"></see>.
        /// Assigns results to a new <see cref="Variable"></see> and returns that variable.
        /// </summary>
        private static Variable Calculate(Variable value1, Variable value2, Func<double, double, double> calculator)
        {
            Variable var = new Variable(value1);
            var.Calculate(value2, calculator);
            return var;
        }

        /// <summary>
        /// <see cref="GetNumericValue(string, out double)"></see> return values.
        /// </summary>
        private enum NumericType
        {
            None,
            Integer,
            Double,
        }

        /// <summary>
        /// Gets the numeric value of this variable.
        /// </summary>
        /// <param name="value">Returns the current value, or 0 if this variable
        /// does not currently contain a numeric value.</param>
        /// <returns>Returns an enum indicating the number type.</returns>
        private NumericType GetNumericValue(out double value)
        {
            switch (Type)
            {
                case VariableType.Integer:
                    value = IntegerValue;
                    return NumericType.Integer;
                case VariableType.Double:
                    value = DoubleValue;
                    return NumericType.Double;
                case VariableType.String:
                    return GetNumericValue(StringValue, out value);
                default:
                    Debug.Assert(false);
                    value = default;
                    return NumericType.None;
            }
        }

        /// <summary>
        /// Gets the numeric value of a string.
        /// </summary>
        /// <param name="s">The string to get the value of.</param>
        /// <param name="value">Returns the value of <paramref name="s"/>, or 0
        /// if it does not contain a value.</param>
        /// <returns>Returns an enum indicating the number type.</returns>
        private static NumericType GetNumericValue(string s, out double value)
        {
            // Attempt to parse int
            if (int.TryParse(s, out int i))
            {
                value = i;
                return NumericType.Integer;
            }
            // Attempt to parse double
            if (double.TryParse(s, out value))
                return NumericType.Double;
            // Not numeric
            value = default;
            return NumericType.None;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return Equals(obj as Variable);
        }

        public bool Equals(Variable other)
        {
            if (other == null)
                return false;

            switch (Type)
            {
                case VariableType.Integer:
                    return other.Type == VariableType.Integer && other.IntegerValue == IntegerValue;
                case VariableType.Double:
                    return other.Type == VariableType.Double && other.DoubleValue == DoubleValue;
                case VariableType.String:
                    return other.Type == VariableType.String && other.StringValue == StringValue;
                default:
                    Debug.Assert(false);
                    return false;
            }
        }

        /// <summary>
        /// Computes a hash code on the current type and value.
        /// </summary>
        public override int GetHashCode()
        {
            var hashCode = 1055843540;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            switch (Type)
            {
                case VariableType.Integer:
                    hashCode = hashCode * -1521134295 + IntegerValue.GetHashCode();
                    break;
                case VariableType.Double:
                    hashCode = hashCode * -1521134295 + DoubleValue.GetHashCode();
                    break;
                case VariableType.String:
                    hashCode = hashCode * -1521134295 + StringValue.GetHashCode();
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            return hashCode;
        }
    }
}
