# TradeOgre
Library for accessing the [TradeOgre Exchange](https://tradeogre.com) api  
  
Simple using:  
Create connector for TradeOgre
```
var connector = new TradeOgreConnect(apiKey, apiSecret);
```  
* connector.Account - Contains account balance
* connector.Exchange - Exchange operations for you
* connector.Market - Market data such as current order book for currency, historical data of the most recent trades, ticker for a trading pair (volume, high, and low are in the last 24 hours, initialprice is the price from 24 hours ago), all listing pairs