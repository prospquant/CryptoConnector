# CryptoConnector
Example of BinanceFutures exchange wrapper

The BinanceFuturesClientConnector class is a part of a trading automation system that provides a connection to the Binance Futures exchange. This connector wraps the necessary functionalities of the Binance Futures API, allowing for easier interaction and extraction of data.

Overview
This connector class inherits from the AbstractConnector and provides a set of functionalities that are specific to the Binance Futures exchange, including:

Initialization: Setting up essential configurations such as endpoints, converters, parameter builders, and more.

REST Public Data: Fetching public data from Binance Futures like server time, active symbols, and tickers.

REST Private Data: Setting leverage and fetching balances for a specific account.

Socket Private Data: Real-time data subscriptions to positions and account balances.

Features

Multiple Constructors: The class provides two constructors to allow for initialization with just a connector name or with API keys as well.

Socket & REST Client Initialization: Setting up REST client and Socket client for both real-time and non-real-time data.

Account Updater: A built-in AccountUpdater to update the account balance and positions at defined intervals.

Public Data Retrieval: Functions to get public data such as server time, available trading pairs, and tickers.

Private Data Manipulation: Set leverage for a specific trading pair and fetch balance information for the logged-in account.

Real-time Position Subscription: Subscribe or unsubscribe to real-time position updates via websockets.

How to Use

Initialization:

Use one of the constructors to initialize the connector.
Example: var connector = new BinanceFuturesClientConnector("myConnectorName", "myApiKey", "mySecretKey");

Fetching Public Data:

Server Time: await connector.GetServerTimeAsync();

Available Trading Pairs: await connector.GetSymbolsAsync(100);

Active Trading Pairs: await connector.GetActiveSymbolsAsync();

Ticker for a Pair: await connector.GetTickerAsync("BTCUSDT");

Fetching & Manipulating Private Data:

Get Account Balance: await connector.GetBalanceAsync();

Socket Subscription:

Subscribe to Position Updates: connector.SubscribeToPosition();

Unsubscribe to Position Updates: connector.UnsubscribeToPosition();
