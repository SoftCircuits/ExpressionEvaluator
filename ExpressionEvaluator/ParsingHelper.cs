// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Linq;
using System.Text;

namespace SoftCircuits.ExpressionEvaluator
{
    /// <summary>
    /// This class provides many methods and conveniences for parsing text. In particular,
    /// it tracks your current location in the text being parsed. It also guards against
    /// any out-of-range exceptions when inspecting text characters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class can be used when doing any sort of significant parsing on a string.
    /// Just create an instance of the <see cref="ParsingHelper"/> class, passing the
    /// string you want to parse to the constructor.
    /// </para>
    /// <para>
    /// Use the <see cref="MoveAhead()"/> method to advance one character. Use the
    /// <see cref="Peek()"/> method to read the character at the current position, and
    /// the <see cref="Peek(int)"/> method to read a character ahead of the current
    /// position. These methods both return <see cref="NullChar"/> if they reference a
    /// character beyond the end of the input string. You can use the
    /// <see cref="EndOfText"/> property to test when the current position is at the
    /// end of the text.
    /// </para>
    /// <para>
    /// In some cases, it may make sense to derive your entire class from
    /// <see cref="ParsingHelper"/>.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example parses a string and extracts all sequences of
    /// alphabetic characters (letters) and adds them to a collection called
    /// <c>tokens</c>.
    /// <code>
    /// string text = " abc!! 77def# ghi ";
    /// ParsingHelper helper = new ParsingHelper(text);
    /// List&lt;string&gt; tokens = new List&lt;string&gt;();
    /// 
    /// while (!helper.EndOfText)
    /// {
    ///     if (char.IsLetter(helper.Peek()))
    ///     {
    ///         int start = helper.Index;
    ///         helper.MoveAhead();
    ///         while (char.IsLetter(helper.Peek()))
    ///             helper.MoveAhead();
    ///         tokens.Add(helper.Extract(start, helper.Index));
    ///     }
    ///     helper.MoveAhead();
    /// }
    /// </code>
    /// </example>
    internal class ParsingHelper
    {
        /// <summary>
        /// Represents an invalid character. This character is returned when a valid character
        /// is not available, such as when returning a character beyond the end of the text.
        /// </summary>
        /// <remarks>
        /// The character is represented internally as <c>'\0'</c>.
        /// </remarks>
        public const char NullChar = '\0';

        /// <summary>
        /// Returns the current text being parsed.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Returns the current position within the text being parsed.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Returns the number of characters not yet parsed. This is equal to the length of the
        /// text being parsed minus the current position within that text.
        /// </summary>
        /// <remarks>
        /// Returns the length of the current text being parsed minus the current position.
        /// </remarks>
        public int Remaining => Text.Length - Index;

        /// <summary>
        /// Constructs a TextParse instance.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        public ParsingHelper(string text = null)
        {
            Reset(text);
        }

        /// <summary>
        /// Resets the current position to the start of the current text.
        /// </summary>
        public void Reset()
        {
            Index = 0;
        }

        /// <summary>
        /// Sets the text to be parsed and resets the current position to the start of that text.
        /// </summary>
        /// <param name="text">The text to be parsed.</param>
        public void Reset(string text)
        {
            Text = text ?? string.Empty;
            Index = 0;
        }

        /// <summary>
        /// Indicates if the current position is at the end of the text being parsed.
        /// </summary>
        public bool EndOfText => (Index >= Text.Length);

        /// <summary>
        /// Returns the character at the current position, or <see cref="NullChar"/>
        /// if we're at the end of the text being parsed.
        /// </summary>
        /// <returns>The character at the current position.</returns>
        public char Peek() => Peek(0);

        /// <summary>
        /// Returns the character at the specified number of characters beyond the current
        /// position, or <see cref="NullChar"/> if the specified position is beyond the
        /// end of the text being parsed.
        /// </summary>
        /// <param name="ahead">The number of characters beyond the current position.</param>
        /// <returns>The character at the specified position.</returns>
        public char Peek(int ahead)
        {
            int pos = (Index + ahead);
            return (pos < Text.Length) ? Text[pos] : NullChar;
        }

        /// <summary>
        /// Extracts a substring of the text being parsed. The substring includes all characters
        /// from the specified position to the end of the text.
        /// </summary>
        /// <param name="start">0-based position of first character to extract.</param>
        /// <returns>Returns the extracted string.</returns>
        public string Extract(int start) => Extract(start, Text.Length);

        /// <summary>
        /// Extracts a substring from the specified range of the text being parsed.
        /// </summary>
        /// <param name="start">0-based position of first character to extract.</param>
        /// <param name="end">0-based position of the character that follows the last
        /// character to extract.</param>
        /// <returns>Returns the extracted string</returns>
        public string Extract(int start, int end) => Text.Substring(start, end - start);

        /// <summary>
        /// Moves the current position ahead one character. The position will not
        /// be placed beyond the end of the text being parsed.
        /// </summary>
        public void MoveAhead() => MoveAhead(1);

        /// <summary>
        /// Moves the current position ahead the specified number of characters. The position
        /// will not be placed beyond the end of the text being parsed.
        /// </summary>
        /// <param name="ahead">The number of characters to move ahead</param>
        public void MoveAhead(int ahead)
        {
            Index = Math.Min(Index + ahead, Text.Length);
        }

        /// <summary>
        /// Moves to the next occurrence of the specified string within the text being parsed.
        /// </summary>
        /// <param name="s">String to find.</param>
        /// <param name="ignoreCase">Indicates if case-insensitive comparisons are used</param>
        public void MoveTo(string s, bool ignoreCase = false)
        {
            Index = Text.IndexOf(s, Index, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            if (Index < 0)
                Index = Text.Length;
        }

        /// <summary>
        /// Moves to the next occurrence of any one of the specified
        /// characters.
        /// </summary>
        /// <param name="chars">Array of characters to search for.</param>
        public void MoveTo(params char[] chars)
        {
            Index = Text.IndexOfAny(chars, Index);
            if (Index < 0)
                Index = Text.Length;
        }

        /// <summary>
        /// Moves the current position to the first character that is part of a newline.
        /// </summary>
        public void MoveToEndOfLine() => MoveTo('\r', '\n');

        /// <summary>
        /// Moves to the next occurrence of any character that is not one
        /// of the specified characters.
        /// </summary>
        /// <param name="chars">Array of characters to skip over</param>
        public void Skip(params char[] chars)
        {
            while (!EndOfText && chars.Contains(Peek()))
                MoveAhead();
        }

        /// <summary>
        /// Moves the current position to the next character that is not a whitespace.
        /// </summary>
        public void SkipWhitespace()
        {
            while (char.IsWhiteSpace(Peek()))
                MoveAhead();
        }

        /// <summary>
        /// Moves the current text position to the next character for which
        /// the given predicate returns false.
        /// </summary>
        /// <param name="predicate">Method that returns true if the character
        /// should be skipped.</param>
        public void SkipWhile(Func<char, bool> predicate)
        {
            while (predicate(Peek()) && !EndOfText)
                MoveAhead();
        }

        /// <summary>
        /// Moves the current text position to the next character for which
        /// the given predicate returns false. And returns a string with
        /// the characters that were skipped.
        /// </summary>
        /// <param name="predicate">Method that returns true if the character
        /// should be skipped.</param>
        /// <returns>A string with the characters that were skipped.</returns>
        public string ParseWhile(Func<char, bool> predicate)
        {
            int start = Index;
            while (predicate(Peek()) && !EndOfText)
                MoveAhead();
            return Extract(start, Index);
        }

        /// <summary>
        /// Moves to the end of quoted text and returns the text within the quotes. Discards the
        /// quote characters. Assumes the character at the current position is the quote character.
        /// Two consecutive quotes are treated as a single literal character (rather than the end
        /// of the quoted text).
        /// </summary>
        /// <returns>Returns the text that was inside of the quotes.</returns>
        public string ParseQuotedText()
        {
            // Get quote character
            char quote = Peek();
            // Jump to start of quoted text
            MoveAhead();
            // Parse quoted text
            StringBuilder builder = new StringBuilder();
            while (!EndOfText)
            {
                int start = Index;
                // Move to next quote
                MoveTo(quote);
                // Capture quoted text
                builder.Append(Extract(start, Index));
                // Skip over quote
                MoveAhead();
                // Two consecutive quotes treated as quote literal
                if (Peek() == quote)
                {
                    builder.Append(quote);
                    MoveAhead();
                }
                else break; // Done if single closing quote
            }
            return builder.ToString();
        }
    }
}
