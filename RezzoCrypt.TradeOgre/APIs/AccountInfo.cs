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

using RezzoCrypt.TradeOgre.Entities.Account;

namespace RezzoCrypt.TradeOgre.APIs
{
    public class AccountInfo
    {
        protected TradeOgreConnect Connect { get; }

        public AccountInfo(TradeOgreConnect _connect)
        {
            Connect = _connect;
        }

        /// <summary>
        /// Retrieve all balances for your account
        /// </summary>
        public CurrencyBalances Balances() => Connect.GetUrlResult<CurrencyBalances>("/account/balances", secure: true);

        /// <summary>
        /// Get the balance of a specific currency for you account
        /// </summary>
        /// <param name="currency">Symbol of a currency</param>
        public CurrencyBalance Balance(string currency)
        {
            var result = Connect.GetUrlResult<CurrencyBalance>("/account/balance", new { currency }, TradeOgreConnect.Method.Post, true);
            if (result.Success)
                result.Currency = currency;

            return result;
        }
    }
}
