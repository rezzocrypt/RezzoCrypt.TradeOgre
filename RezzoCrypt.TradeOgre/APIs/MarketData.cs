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

using RezzoCrypt.TradeOgre.Entities.Market;
using RezzoCrypt.TradeOgre.Extensions;

namespace RezzoCrypt.TradeOgre.APIs
{
    public class MarketData
    {
        protected TradeOgreConnect Connect { get; }

        public MarketData(TradeOgreConnect _connect)
        {
            Connect = _connect;
        }

        /// <summary>
        /// Retrieve the current order book for currency
        /// </summary>
        /// <param name="currency1">First currency pair</param>
        /// <param name="currency2">Second currency pair</param>
        public OrderBookExchange CurrentOrderBook(string currency1, string currency2) => Connect.GetUrlResult<OrderBookExchange>($"/orders/{OgrePrepareExtensions.MakePair(currency1, currency2)}");

        /// <summary>
        /// Retrieve the history of the last trades limited to 100 of the most recent trades
        /// </summary>
        /// <param name="currency1">First currency pair</param>
        /// <param name="currency2">Second currency pair</param>
        public TradeHistory[] HistorycalData(string currency1, string currency2) => Connect.GetUrlResult<TradeHistory[]>($"/history/{OgrePrepareExtensions.MakePair(currency1, currency2)}");

        /// <summary>
        /// Retrieve the ticker for a trading pair, volume, high, and low are in the last 24 hours, initialprice is the price from 24 hours ago.
        /// </summary>
        /// <param name="currency1">First currency pair</param>
        /// <param name="currency2">Second currency pair</param>
        public Ticker Tiker(string currency1, string currency2)
        {
            var result = Connect.GetUrlResult<Ticker>($"/ticker/{OgrePrepareExtensions.MakePair(currency1, currency2)}");
            if (result.Success)
            {
                result.Currency1 = currency1;
                result.Currency2 = currency2;
            }

            return result;
        }

        /// <summary>
        /// Retrieve a listing of all markets and basic information including current price, volume, high, low, bid and ask.
        /// </summary>
        public Ticker[] ListingPairs()
        {
            var result = Connect.GetUrlResult<Dictionary<string, Ticker>[]>($"/markets");
            return result?
                .SelectMany(item => item)
                .Select(item =>
            {
                item.Value.Currency1 = OgrePrepareExtensions.ParcePair(item.Key);
                item.Value.Currency2 = OgrePrepareExtensions.ParcePair(item.Key, false);
                return item.Value;
            }).ToArray() ?? Array.Empty<Ticker>();
        }
    }
}
