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

using Newtonsoft.Json;
using RezzoCrypt.TradeOgre.Entities.Enums;
using RezzoCrypt.TradeOgre.Extensions;

namespace RezzoCrypt.TradeOgre.Entities.Exchange
{
    public class Order : BaseApiResponse
    {
        [JsonProperty(PropertyName = "uuid")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "type")]
        public TradeSide Side { get; set; }

        public long Date { get; set; }

        public DateTime RealDate => OgrePrepareExtensions.UnixTimeStampToDateTime(Date);
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "market")]
        public string Pair { get; set; } = string.Empty;

        public string Currency1 => OgrePrepareExtensions.ParcePair(Pair);
        public string Currency2 => OgrePrepareExtensions.ParcePair(Pair, false);
    }
}
