using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// String manipulation and generation methods, as well as string array manipulation.
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static char[] DefaultQuoteSensitiveChars = new char[] { '\"' };

        private static Random s_random;

        /// <summary>
        /// 
        /// </summary>
        static StringUtilities()
        {
            s_random = new Random(unchecked((int)DateTime.UtcNow.Ticks));
        }

        /// <summary>
        /// Determines whether [is string null or empty with trim] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string null or empty with trim] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringNullOrEmptyWithTrim(string str)
        {
            if (str == null)
            {
                return true;
            }

            str = str.Trim();
            return str.Length == 0;
        }

        /// <summary>
        /// Joins.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="chars">The chars.</param>
        /// <returns></returns>
        public static string Join(string separator, params char[] chars)
        {
            string result = null;

            if (chars != null)
            {
                int l = chars.Length;
                for (int i = 0; i < l; i++)
                {
                    if (i > 0)
                    {
                        result += separator;
                    }
                    result += chars[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the bytes from string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static byte[] GetBytesFromString(string str)
        {
            // Strings in .NET are always UTF16
            return Encoding.Unicode.GetBytes(str);
        }

        /// <summary>
        /// Gets the string from bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetStringFromBytes(byte[] data)
        {
            return Encoding.Unicode.GetString(data);
        }

        /// <summary>
        /// Returns a string of length <paramref name="size"/> filled
        /// with random ASCII characters in the range A-Z, a-z. If <paramref name="lowerCase"/>
        /// is <c>true</c>, then the range is only a-z.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="lowerCase">if set to <c>true</c> [lower case].</param>
        /// <returns></returns>
        public static string RandomString(int size, bool lowerCase)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", "Size must be positive");
            }
            StringBuilder builder = new StringBuilder(size);
            int low = 65; // 'A'
            int high = 91; // 'Z' + 1
            if (lowerCase)
            {
                low = 97; // 'a';
                high = 123; // 'z' + 1
            }
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(s_random.Next(low, high));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a string of length <paramref name="length"/> with
        /// 0's padded to the left, if necessary.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length)
        {
            return PadIntegerLeft(val, length, '0');
        }

        /// <summary>
        /// Pads the integer left.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <param name="pad">The pad.</param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length, char pad)
        {
            string result = val.ToString();
            while (result.Length < length)
            {
                result = pad + result;
            }
            return result;
        }

        /// <summary>
        /// Returns a string of length <paramref name="length"/> with
        /// 0's padded to the right, if necessary.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadIntegerRight(int val, int length)
        {
            return PadIntegerRight(val, length, '0');
        }

        /// <summary>
        /// Pads the integer right.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <param name="pad">The pad.</param>
        /// <returns></returns>
        public static string PadIntegerRight(int val, int length, char pad)
        {
            string result = val.ToString();
            while (result.Length < length)
            {
                result += pad;
            }
            return result;
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> (case sensitive) with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace)
        {
            return ReplaceFirst(str, find, replace, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="findComparison">The find comparison.</param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace, StringComparison findComparison)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            else if (string.IsNullOrEmpty(find))
            {
                throw new ArgumentNullException("find");
            }
            int firstIndex = str.IndexOf(find, findComparison);
            if (firstIndex != -1)
            {
                if (firstIndex == 0)
                {
                    str = replace + str.Substring(find.Length);
                }
                else if (firstIndex == (str.Length - find.Length))
                {
                    str = str.Substring(0, firstIndex) + replace;
                }
                else
                {
                    str = str.Substring(0, firstIndex) + replace + str.Substring(firstIndex + find.Length);
                }
            }
            return str;
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the first location of <paramref name="ch"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="ch">The character to find.</param>
        /// <returns></returns>
        public static string[] SplitAroundIndexOf(string str, char ch)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.IndexOf(ch));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the first location of any of the characters from
        /// <paramref name="anyOf"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="anyOf">Any of.</param>
        /// <returns></returns>
        public static string[] SplitAroundIndexOfAny(string str, params char[] anyOf)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.IndexOfAny(anyOf));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the last location of <paramref name="ch"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="ch">The character to find.</param>
        /// <returns></returns>
        public static string[] SplitAroundLastIndexOf(string str, char ch)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.LastIndexOf(ch));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the last location of any of the charactesr from
        /// <paramref name="anyOf"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="anyOf">Any of.</param>
        /// <returns></returns>
        public static string[] SplitAroundLastIndexOfAny(string str, params char[] anyOf)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.LastIndexOfAny(anyOf));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on the index. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static string[] SplitAround(string str, int index)
        {
            string one, two;
            if (index == -1)
            {
                one = "";
                two = str;
            }
            else
            {
                if (index == 0)
                {
                    one = "";
                    two = str.Substring(1);
                }
                else if (index == str.Length - 1)
                {
                    one = str.Substring(0, str.Length - 1);
                    two = "";
                }
                else
                {
                    one = str.Substring(0, index);
                    two = str.Substring(index + 1);
                }
            }

            return new string[] { one, two };
        }

        /// <summary>
        /// Splits the specified pieces.
        /// </summary>
        /// <param name="pieces">The pieces.</param>
        /// <param name="splitChar">The split char.</param>
        /// <param name="indices">The indices.</param>
        /// <returns></returns>
        public static string[] Split(string[] pieces, char splitChar, params int[] indices)
        {
            if (pieces == null)
            {
                throw new ArgumentNullException("pieces");
            }

            if (indices != null && indices.Length == 0)
            {
                indices = new int[pieces.Length];
                for (int k = 0; k < indices.Length; k++)
                {
                    indices[k] = k;
                }
            }

            // First, we need to sort the indices
            Array.Sort(indices);

            int offset = 0;
            if (indices != null)
            {
                foreach (int index in indices)
                {
                    if (index + offset < pieces.Length)
                    {
                        string[] subPieces = pieces[index + offset].Split(splitChar);
                        if (subPieces.Length > 1)
                        {
                            pieces = ArrayUtilities.InsertReplace<string>(pieces, index + offset, subPieces);
                            offset += subPieces.Length - 1;
                        }
                    }
                }
            }

            return pieces;
        }

        /// <summary>
        /// Splits the string based on whitespace, being sensitive to
        /// quotes. Always returns a non-null array, possibly zero-length.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="dividerChars">The divider chars.</param>
        /// <returns></returns>
        public static string[] SplitQuoteSensitive(string line, params char[] dividerChars)
        {
            return SplitQuoteSensitive(line, false, dividerChars);
        }

        /// <summary>
        /// Splits the string based on whitespace, being sensitive to
        /// quotes. Always returns a non-null array, possibly zero-length.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="retainDivider">if set to <c>true</c> [retain divider].</param>
        /// <param name="dividerChars">The divider chars.</param>
        /// <returns></returns>
        public static string[] SplitQuoteSensitive(string line, bool retainDivider, params char[] dividerChars)
        {
            List<string> result = new List<string>();
            if (line != null)
            {
                if (dividerChars == null || dividerChars.Length == 0)
                {
                    // no divider chars specified, use the default
                    dividerChars = DefaultQuoteSensitiveChars;
                }

                SplitQuoteSensitiveState state = SplitQuoteSensitiveState.InEther;
                int length = line.Length;
                char c;
                StringBuilder sb = new StringBuilder(length);
                char matchChar = '\0';

                for (int i = 0; i < length; i++)
                {
                    c = line[i];
                    if (char.IsWhiteSpace(c))
                    {
                        switch (state)
                        {
                            case SplitQuoteSensitiveState.InPiece:
                                // the piece has ended
                                result.Add(sb.ToString());
                                sb.Length = 0;
                                state = SplitQuoteSensitiveState.InEther;
                                break;
                            case SplitQuoteSensitiveState.InDivision:
                                // whitespace within quotes
                                sb.Append(c);
                                break;

                            // ignore:
                            //case SplitQuoteSensitiveState.InEther:
                        }
                    }
                    else if (CharUtilities.IsCharacterOneOf(c, dividerChars) && (matchChar == '\0' || matchChar == c))
                    {
                        switch (state)
                        {
                            case SplitQuoteSensitiveState.InEther:
                                state = SplitQuoteSensitiveState.InDivision;
                                matchChar = c;
                                if (retainDivider)
                                {
                                    sb.Append(c);
                                }
                                break;
                            case SplitQuoteSensitiveState.InPiece:
                                // quote in the middle of a piece
                                sb.Append(c);
                                break;
                            case SplitQuoteSensitiveState.InDivision:
                                // Finish the piece
                                result.Add(sb.ToString());
                                sb.Length = 0;
                                matchChar = '\0';
                                state = SplitQuoteSensitiveState.InEther;
                                if (retainDivider)
                                {
                                    sb.Append(c);
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (state == SplitQuoteSensitiveState.InEther)
                        {
                            state = SplitQuoteSensitiveState.InPiece;
                        }
                        sb.Append(c);
                    }
                }

                // See if there is any trailing content
                switch (state)
                {
                    case SplitQuoteSensitiveState.InPiece:
                    case SplitQuoteSensitiveState.InDivision:
                        result.Add(sb.ToString());
                        break;
                }
            }
            return result.ToArray();
        }

        private enum SplitQuoteSensitiveState
        {
            InEther,
            InPiece,
            InDivision
        }

        /// <summary>
        /// Removes the empty pieces.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static string[] RemoveEmptyPieces(string[] array)
        {
            int index = IndexOfEmptyPiece(array);
            while (index != -1)
            {
                array = ArrayUtilities.Remove<string>(array, index);
                index = IndexOfEmptyPiece(array, index);
            }
            return array;
        }

        /// <summary>
        /// Indexes the of empty piece.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static int IndexOfEmptyPiece(string[] array)
        {
            return IndexOfEmptyPiece(array, 0);
        }

        /// <summary>
        /// Indexes the of empty piece.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOfEmptyPiece(string[] array, int startIndex)
        {
            for (int i = startIndex; i < array.Length; i++)
            {
                if (string.IsNullOrEmpty(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
