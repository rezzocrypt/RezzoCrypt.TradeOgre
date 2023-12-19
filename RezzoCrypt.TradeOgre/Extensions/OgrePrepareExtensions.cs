/*
 *   Copyright (c) 2023 Alexey Vinogradov
 *   All rights reserved.

 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 */

namespace RezzoCrypt.TradeOgre.Extensions
{
    internal static class OgrePrepareExtensions
    {
        /// <summary>
        /// Current pair delimeter
        /// </summary>
        static readonly string pairDelimeter = "-";

        /// <summary>
        /// Make market pair by two currency
        /// </summary>
        /// <param name="currency1">First currency pair</param>
        /// <param name="currency2">Second currency pair</param>
        public static string MakePair(string currency1, string currency2) => $"{currency1}{pairDelimeter}{currency2}";

        /// <summary>
        /// Parce pair data
        /// </summary>
        /// <param name="pair">Market pair</param>
        /// <param name="needFirstCurrency">Need first currency?</param>
        public static string ParcePair(string pair, bool needFirstCurrency = true)
        {
            if (string.IsNullOrEmpty(pair))
                return "";

            var array = pair.Split(pairDelimeter);
            if (array.Length != 2)
                return "";

            return needFirstCurrency ? array[0] : array[1];
        }

        /// <summary>
        /// Converl UnixTimeTicks to DateTime
        /// </summary>
        /// <param name="unixTimeStamp">UnixTimestamp</param>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
