// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
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

        private enum NumericType
        {
            None,
            Integer,
            Double,
        }

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be added.</param>
        public void Add(int operand)
        {
            if (GetNumericValue(out double value) == NumericType.Double)
                SetValue(value + operand);
            else
                SetValue((int)value + operand);
        }

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be added.</param>
        public void Add(double operand) => SetValue(ToDouble() + operand);

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be added.</param>
        public void Add(string operand) => CalculateStringOperand(operand, (val, op) => val + op);

        /// <summary>
        /// Adds the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be added.</param>
        public void Add(Variable operand)
        {
            switch (operand.Type)
            {
                case VariableType.Integer:
                    Add(operand.IntegerValue);
                    break;
                case VariableType.Double:
                    Add(operand.DoubleValue);
                    break;
                case VariableType.String:
                    Add(operand.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be subtracted.</param>
        public void Subtract(int operand)
        {
            if (GetNumericValue(out double value) == NumericType.Double)
                SetValue(value - operand);
            else
                SetValue((int)value - operand);
        }

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be subtracted.</param>
        public void Subtract(double operand) => SetValue(ToDouble() - operand);

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be subtracted.</param>
        public void Subtract(string operand) => CalculateStringOperand(operand, (val, op) => val - op);

        /// <summary>
        /// Subtracts the given value to this Variable.
        /// </summary>
        /// <param name="operand">The value to be subtracted.</param>
        public void Subtract(Variable operand)
        {
            switch (operand.Type)
            {
                case VariableType.Integer:
                    Subtract(operand.IntegerValue);
                    break;
                case VariableType.Double:
                    Subtract(operand.DoubleValue);
                    break;
                case VariableType.String:
                    Subtract(operand.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="operand">The value by which to multiply.</param>
        public void Multiply(int operand)
        {
            if (GetNumericValue(out double value) == NumericType.Double)
                SetValue(value * operand);
            else
                SetValue((int)value * operand);
        }

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="operand">The value by which to multiply.</param>
        public void Multiply(double operand) => SetValue(ToDouble() * operand);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="operand">The value by which to multiply.</param>
        public void Multiply(string operand) => CalculateStringOperand(operand, (val, op) => val * op);

        /// <summary>
        /// Multiplies this Variable by the given value.
        /// </summary>
        /// <param name="operand">The value by which to multiply.</param>
        public void Multiply(Variable operand)
        {
            switch (operand.Type)
            {
                case VariableType.Integer:
                    Multiply(operand.IntegerValue);
                    break;
                case VariableType.Double:
                    Multiply(operand.DoubleValue);
                    break;
                case VariableType.String:
                    Multiply(operand.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Divide(int operand)
        {
            if (GetNumericValue(out double value) == NumericType.Double)
                SetValue((operand == 0) ? 0 : value / operand);
            else
                SetValue((operand == 0) ? 0 : (int)value / operand);
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Divide(double operand) => SetValue((operand == 0) ? 0 : ToDouble() / operand);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Divide(string operand) => CalculateStringOperand(operand, (val, op) => (op == 0) ? 0 : val / op);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Divide(Variable operand)
        {
            switch (operand.Type)
            {
                case VariableType.Integer:
                    Divide(operand.IntegerValue);
                    break;
                case VariableType.Double:
                    Divide(operand.DoubleValue);
                    break;
                case VariableType.String:
                    Divide(operand.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Modulus(int operand)
        {
            if (GetNumericValue(out double value) == NumericType.Double)
                SetValue((operand == 0) ? 0 : value % operand);
            else
                SetValue((operand == 0) ? 0 : (int)value % operand);
        }

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Modulus(double operand) => SetValue((operand == 0) ? 0 : ToDouble() % operand);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Modulus(string operand) => CalculateStringOperand(operand, (val, op) => (op == 0) ? 0 : val % op);

        /// <summary>
        /// Divides this Variable by the given value. Returns zero if the divisor is zero.
        /// </summary>
        /// <param name="operand">The value by which to divide.</param>
        public void Modulus(Variable operand)
        {
            switch (operand.Type)
            {
                case VariableType.Integer:
                    Modulus(operand.IntegerValue);
                    break;
                case VariableType.Double:
                    Modulus(operand.DoubleValue);
                    break;
                case VariableType.String:
                    Modulus(operand.StringValue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="operand">The value to be concatenated.</param>
        public void Concatenate(int operand) => SetValue(ToString() + operand.ToString());

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="operand">The value to be concatenated.</param>
        public void Concatenate(double operand) => SetValue(ToString() + operand.ToString());

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="operand">The value to be concatenated.</param>
        public void Concatenate(string operand) => SetValue(ToString() + operand);

        /// <summary>
        /// Concatenates two values together as strings.
        /// </summary>
        /// <param name="operand">The value to be concatenated.</param>
        public void Concatenate(Variable operand) => SetValue(ToString() + operand.ToString());

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
                    if (GetNumericValue(StringValue, out double value) == NumericType.Double)
                        SetValue(-value);
                    else
                        SetValue(-(int)value);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Performs a calculation using this value and a string value.
        /// </summary>
        /// <param name="s">The string value to use in the calculation.</param>
        /// <param name="calc">Delegate that performs the calculation.</param>
        private void CalculateStringOperand(string s, Func<double, double, double> calc)
        {
            // Get values
            var valType = GetNumericValue(out double value);
            var opType = GetNumericValue(s, out double operand);

            // Perform calculation
            value = calc(value, operand);

            // Set result
            if (valType == NumericType.Double || opType == NumericType.Double)
                SetValue(value);
            else
                SetValue((int)value);
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
            if (double.TryParse(s, out double d))
            {
                value = d;
                return NumericType.Double;
            }

            // Not numeric
            value = default;
            return NumericType.None;
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
        public static bool operator ==(Variable v1, string v2) => CompareStrings(v1, v2) == 0;
        public static bool operator ==(Variable v1, Variable v2) => CompareStrings(v1, v2) == 0;

        public static bool operator !=(Variable v1, int v2) => v1.ToInteger() != v2;
        public static bool operator !=(Variable v1, double v2) => v1.ToDouble() != v2;
        public static bool operator !=(Variable v1, string v2) => CompareStrings(v1, v2) != 0;
        public static bool operator !=(Variable v1, Variable v2) => CompareStrings(v1, v2) != 0;

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
            if (v1.GetNumericValue(out double value1) != NumericType.None &&
                v2.GetNumericValue(out double value2) != NumericType.None)
                return value1 - value2;
            // Else compare as strings
            return string.CompareOrdinal(v1.ToString(), v2.ToString());
        }

        private static double CompareStrings(Variable v1, string v2)
        {
            // Compare as numbers if possible
            if (v1.GetNumericValue(out double value1) != NumericType.None &&
                GetNumericValue(v2, out double value2) != NumericType.None)
                return value1 - value2;
            // Else compare as strings
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
