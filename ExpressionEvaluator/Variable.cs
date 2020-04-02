﻿// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Diagnostics;

namespace SoftCircuits.ExpressionEvaluator
{
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
            IntegerValue = value;
            Type = VariableType.Integer;
        }

        /// <summary>
        /// Sets this variable to the specified value.
        /// </summary>
        /// <param name="value">The value to assign to this Variable.</param>
        public void SetValue(double value)
        {
            DoubleValue = value;
            Type = VariableType.Double;
        }

        /// <summary>
        /// Sets this variable to the specified value.
        /// </summary>
        /// <param name="value">The value to assign to this Variable.</param>
        public void SetValue(string value)
        {
            StringValue = value ?? string.Empty;
            Type = VariableType.String;
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
                    if (double.TryParse(StringValue, out double dbl))
                        return (int)Math.Round(dbl);
                    return default;
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
        /// Adds the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(int value)
        {
            if (IsFloatingPoint())
                SetValue(ToDouble() + value);
            else
                SetValue(ToInteger() + value);
        }

        /// <summary>
        /// Adds the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(double value) => SetValue(ToDouble() + value);

        /// <summary>
        /// Adds the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(string value) => StringOperation(value, (x, y) => x + y);

        /// <summary>
        /// Adds the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(Variable value)
        {
            switch (value.Type)
            {
                case VariableType.Integer:
                    Add(value.IntegerValue);
                    break;
                case VariableType.Double:
                    Add(value.DoubleValue);
                    break;
                case VariableType.String:
                    Add(value.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Subtracts the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(int value)
        {
            if (IsFloatingPoint())
                SetValue(ToDouble() - value);
            else
                SetValue(ToInteger() - value);
        }

        /// <summary>
        /// Subtracts the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(double value) => SetValue(ToDouble() - value);

        /// <summary>
        /// Subtracts the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(string value) => StringOperation(value, (x, y) => x - y);

        /// <summary>
        /// Subtracts the give value to this Variable.
        /// </summary>
        /// <param name="value">The value to be subtracted.</param>
        public void Subtract(Variable value)
        {
            switch (value.Type)
            {
                case VariableType.Integer:
                    Subtract(value.IntegerValue);
                    break;
                case VariableType.Double:
                    Subtract(value.DoubleValue);
                    break;
                case VariableType.String:
                    Subtract(value.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(int value)
        {
            if (IsFloatingPoint())
                SetValue(ToDouble() * value);
            else
                SetValue(ToInteger() * value);
        }

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(double value) => SetValue(ToDouble() * value);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(string value) => StringOperation(value, (x, y) => x * y);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="value">The value by which to multiply.</param>
        public void Multiply(Variable value)
        {
            switch (value.Type)
            {
                case VariableType.Integer:
                    Multiply(value.IntegerValue);
                    break;
                case VariableType.Double:
                    Multiply(value.DoubleValue);
                    break;
                case VariableType.String:
                    Multiply(value.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(int value)
        {
            if (IsFloatingPoint())
                SetValue((value == 0) ? 0 : ToDouble() / value);
            else
                SetValue((value == 0) ? 0 : ToInteger() / value);
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(double value) => SetValue((value == 0) ? 0 : ToDouble() / value);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(string value) => StringOperation(value, (x, y) => (y == 0) ? 0 : x / y);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Divide(Variable value)
        {
            switch (value.Type)
            {
                case VariableType.Integer:
                    Divide(value.IntegerValue);
                    break;
                case VariableType.Double:
                    Divide(value.DoubleValue);
                    break;
                case VariableType.String:
                    Divide(value.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="value">The value by which to divide.</param>
        public void Modulus(int value)
        {
            if (IsFloatingPoint())
                SetValue((value == 0) ? 0 : ToDouble() % value);
            else
                SetValue((value == 0) ? 0 : ToInteger() % value);
        }
        public void Modulus(double value) => SetValue((value == 0) ? 0 : ToDouble() % value);
        public void Modulus(string value) => StringOperation(value, (x, y) => (y == 0) ? 0 : x % y);
        public void Modulus(Variable value)
        {
            switch (value.Type)
            {
                case VariableType.Integer:
                    Modulus(value.IntegerValue);
                    break;
                case VariableType.Double:
                    Modulus(value.DoubleValue);
                    break;
                case VariableType.String:
                    Modulus(value.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

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
                    if (int.TryParse(StringValue, out int i))
                        SetValue(-i);
                    else if (double.TryParse(StringValue, out double d))
                        SetValue(-d);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Returns true if this Variable contains a floating point value, or a string
        /// value that represents a floating point value.
        /// </summary>
        private bool IsFloatingPoint()
        {
            GetNumericValue(out double _, out bool isFloat);
            return isFloat;
        }

        /// <summary>
        /// Sets the value of this Variable based on the specified string value
        /// and the <paramref name="op"/> delegate.
        /// </summary>
        /// <param name="s">The string on which to perform the operation.</param>
        /// <param name="op">Delegate that performs the operation.</param>
        private void StringOperation(string s, Func<double, double, double> op)
        {
            // Get values
            GetNumericValue(out double val1, out bool isFloat1);
            GetNumericValue(s, out double val2, out bool isFloat2);

            // Perform operation
            val1 = op(val1, val2);

            // Set result
            if (isFloat1 || isFloat2)
                SetValue(val1);
            else
                SetValue((int)val1);
        }

        /// <summary>
        /// Gets the numeric value of this variable.
        /// </summary>
        /// <param name="value">Returns the current value, or 0 if this variable
        /// does not currently contain a numeric value.</param>
        /// <param name="isFloat">Returns true if the value is a floating point.</param>
        /// <returns>True if this Variable contains a numeric value.</returns>
        private bool GetNumericValue(out double value, out bool isFloat)
        {
            switch (Type)
            {
                case VariableType.Integer:
                    value = IntegerValue;
                    isFloat = false;
                    return true;
                case VariableType.Double:
                    value = DoubleValue;
                    isFloat = true;
                    return true;
                case VariableType.String:
                    return GetNumericValue(StringValue, out value, out isFloat);
                default:
                    Debug.Assert(false);
                    value = default;
                    isFloat = default;
                    return false;
            }
        }

        /// <summary>
        /// Gets the numeric value of a string.
        /// </summary>
        /// <param name="s">The string to get the value of.</param>
        /// <param name="value">Returns the value of <paramref name="s"/>, or 0
        /// if it does not contain a value.</param>
        /// <param name="isFloat">Returns true if the value is floating point.</param>
        /// <returns>Returns true if the string contained a numeric value.</returns>
        private static bool GetNumericValue(string s, out double value, out bool isFloat)
        {
            if (int.TryParse(s, out int i))
            {
                value = i;
                isFloat = false;
                return true;
            }
            if (double.TryParse(s, out double d))
            {
                value = d;
                isFloat = true;
                return true;
            }
            value = default;
            isFloat = default;
            return false;
        }

        #endregion

        #region Conversion operators

        public static implicit operator int(Variable v) => v.ToInteger();
        public static implicit operator double(Variable v) => v.ToDouble();
        public static implicit operator string(Variable v) => v.ToString();

        #endregion

        #region Comparison operators

        public static bool operator ==(Variable v1, int v2) => v1.ToInteger() == v2;
        public static bool operator ==(Variable v1, double v2) => v1.ToDouble() == v2;
        public static bool operator ==(Variable v1, string v2) => v1.ToString() == v2;
        public static bool operator ==(Variable v1, Variable v2) => v1.ToString() == v2.ToString();

        public static bool operator !=(Variable v1, int v2) => v1.ToInteger() != v2;
        public static bool operator !=(Variable v1, double v2) => v1.ToDouble() != v2;
        public static bool operator !=(Variable v1, string v2) => v1.ToString() != v2;
        public static bool operator !=(Variable v1, Variable v2) => v1.ToString() != v2.ToString();

        public static bool operator <(Variable v1, int v2) => v1.ToInteger() < v2;
        public static bool operator <(Variable v1, double v2) => v1.ToDouble() < v2;
        public static bool operator <(Variable v1, string v2) => CompareStrings(v1, v2) < 0;
        public static bool operator <(Variable v1, Variable v2) => CompareStrings(v1, v2) < 0;

        public static bool operator <=(Variable v1, int v2) => v1.ToInteger() <= v2;
        public static bool operator <=(Variable v1, double v2) => v1.ToDouble() <= v2;
        public static bool operator <=(Variable v1, string v2) => CompareStrings(v1, v2) <= 0;
        public static bool operator <=(Variable v1, Variable v2) => CompareStrings(v1, v2) <= 0;

        public static bool operator >(Variable v1, int v2) => v1.ToInteger() > v2;
        public static bool operator >(Variable v1, double v2) => v1.ToDouble() > v2;
        public static bool operator >(Variable v1, string v2) => CompareStrings(v1, v2) > 0;
        public static bool operator >(Variable v1, Variable v2) => CompareStrings(v1, v2) > 0;

        public static bool operator >=(Variable v1, int v2) => v1.ToInteger() >= v2;
        public static bool operator >=(Variable v1, double v2) => v1.ToDouble() >= v2;
        public static bool operator >=(Variable v1, string v2) => CompareStrings(v1, v2) >= 0;
        public static bool operator >=(Variable v1, Variable v2) => CompareStrings(v1, v2) >= 0;

        private static double CompareStrings(Variable v1, Variable v2)
        {
            // Compare as numbers if possible
            if (v1.GetNumericValue(out double value1, out bool _) && v2.GetNumericValue(out double value2, out bool _))
                return value1 - value2;
            return string.CompareOrdinal(v1.ToString(), v2.ToString());
        }

        private static double CompareStrings(Variable v1, string v2)
        {
            // Compare as numbers if possible
            if (v1.GetNumericValue(out double value1, out bool _) && GetNumericValue(v2, out double value2, out bool _))
                return value1 - value2;
            return string.CompareOrdinal(v1.ToString(), v2);
        }

        #endregion

        #region Operation operators

        public static Variable operator +(Variable v1, int v2)
        {
            Variable v = new Variable(v1);
            v.Add(v2);
            return v;
        }
        public static Variable operator +(Variable v1, double v2)
        {
            Variable v = new Variable(v1);
            v.Add(v2);
            return v;
        }
        public static Variable operator +(Variable v1, string v2)
        {
            Variable v = new Variable(v1);
            v.Add(v2);
            return v;
        }
        public static Variable operator +(Variable v1, Variable v2)
        {
            Variable v = new Variable(v1);
            v.Add(v2);
            return v;
        }

        public static Variable operator -(Variable v1, int v2)
        {
            Variable v = new Variable(v1);
            v.Subtract(v2);
            return v;
        }
        public static Variable operator -(Variable v1, double v2)
        {
            Variable v = new Variable(v1);
            v.Subtract(v2);
            return v;
        }
        public static Variable operator -(Variable v1, string v2)
        {
            Variable v = new Variable(v1);
            v.Subtract(v2);
            return v;
        }
        public static Variable operator -(Variable v1, Variable v2)
        {
            Variable v = new Variable(v1);
            v.Subtract(v2);
            return v;
        }

        public static Variable operator *(Variable v1, int v2)
        {
            Variable v = new Variable(v1);
            v.Multiply(v2);
            return v;
        }
        public static Variable operator *(Variable v1, double v2)
        {
            Variable v = new Variable(v1);
            v.Multiply(v2);
            return v;
        }
        public static Variable operator *(Variable v1, string v2)
        {
            Variable v = new Variable(v1);
            v.Multiply(v2);
            return v;
        }
        public static Variable operator *(Variable v1, Variable v2)
        {
            Variable v = new Variable(v1);
            v.Multiply(v2);
            return v;
        }

        public static Variable operator /(Variable v1, int v2)
        {
            Variable v = new Variable(v1);
            v.Divide(v2);
            return v;
        }
        public static Variable operator /(Variable v1, double v2)
        {
            Variable v = new Variable(v1);
            v.Divide(v2);
            return v;
        }
        public static Variable operator /(Variable v1, string v2)
        {
            Variable v = new Variable(v1);
            v.Divide(v2);
            return v;
        }
        public static Variable operator /(Variable v1, Variable v2)
        {
            Variable v = new Variable(v1);
            v.Divide(v2);
            return v;
        }

        public static Variable operator %(Variable v1, int v2)
        {
            Variable v = new Variable(v1);
            v.Modulus(v2);
            return v;
        }
        public static Variable operator %(Variable v1, double v2)
        {
            Variable v = new Variable(v1);
            v.Modulus(v2);
            return v;
        }
        public static Variable operator %(Variable v1, string v2)
        {
            Variable v = new Variable(v1);
            v.Modulus(v2);
            return v;
        }
        public static Variable operator %(Variable v1, Variable v2)
        {
            Variable v = new Variable(v1);
            v.Modulus(v2);
            return v;
        }

        public static Variable operator &(Variable v1, int v2)
        {
            Variable v = new Variable(v1);
            v.Concatenate(v2);
            return v;
        }
        public static Variable operator &(Variable v1, double v2)
        {
            Variable v = new Variable(v1);
            v.Concatenate(v2);
            return v;
        }
        public static Variable operator &(Variable v1, string v2)
        {
            Variable v = new Variable(v1);
            v.Concatenate(v2);
            return v;
        }
        public static Variable operator &(Variable v1, Variable v2)
        {
            Variable v = new Variable(v1);
            v.Concatenate(v2);
            return v;
        }

        public static Variable operator -(Variable v1)
        {
            Variable v = new Variable(v1);
            v.Negate();
            return v;
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
