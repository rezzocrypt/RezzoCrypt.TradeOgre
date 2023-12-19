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

using RezzoCrypt.TradeOgre.Entities.Enums;
using RezzoCrypt.TradeOgre.Entities.Exchange;
using RezzoCrypt.TradeOgre.Extensions;

namespace RezzoCrypt.TradeOgre.APIs
{
    public class AccountExchange
    {
        protected TradeOgreConnect Connect { get; }

        public AccountExchange(TradeOgreConnect _connect)
        {
            Connect = _connect;
        }

        /// <summary>
        /// Retrieve information about a specific order by the uuid of the order
        /// </summary>
        /// <param name="id">Order Id to find</param>
        public Order Order(string id)
        {
            var result = Connect.GetUrlResult<Order>($"/account/order/{id}", secure: true);
            if (result.Success)
                result.Id = id;

            return result;
        }

        /// <summary>
        /// Retrieve the active orders under your account
        /// </summary>
        /// <param name="currency1">First currency pair</param>
        /// <param name="currency2">Second currency pair</param>
        public Order[] Orders(string currency1 = "", string currency2 = "")
        {
            var result = Connect.GetUrlResult<Order[]>(
                $"/account/orders"
                , string.IsNullOrEmpty(currency1) || string.IsNullOrEmpty(currency2)
                    ? null
                    : new { market = OgrePrepareExtensions.MakePair(currency1, currency2) }
                , TradeOgreConnect.Method.Post
                , true);
            return result;
        }

        /// <summary>
        /// Submit order to the order book for a market
        /// </summary>
        /// <param name="currency1">First currency pair</param>
        /// <param name="currency2">Second currency pair</param>
        /// <param name="side">Trade side</param>
        /// <param name="price">Order price</param>
        /// <param name="quantity">Order quantity</param>
        public Order Create(string currency1, string currency2, TradeSide side, decimal price, decimal quantity)
        {
            var result = Connect.GetUrlResult<TradeOrder>(
                side == TradeSide.Sell ? "/order/sell" : "/order/buy",
                new { market = OgrePrepareExtensions.MakePair(currency1, currency2), price, quantity }, TradeOgreConnect.Method.Post, true);

            if (!result.Success)
                return new Order { Success = false, Error = result.Error };

            return Order(result.Id);
        }

        /// <summary>
        /// Cancel an order on the order book based on the order uuid
        /// </summary>
        /// <param name="id">Order Id to cancel</param>
        /// <returns>Is success result</returns>
        public CancelActionResult Cancel(string id) => Connect.GetUrlResult<CancelActionResult>("/order/cancel", new { uuid = id }, TradeOgreConnect.Method.Post, true);
    }
}
