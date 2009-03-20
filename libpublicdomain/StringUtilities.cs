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
