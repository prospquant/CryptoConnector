namespace HQConnector.Dto.DTO.Enums.Exchange
{
    public enum Exchange : int
    {
        None = 0,
        Binance = 1,
        Bitfinex = 2,
        BitMEX = 3,        
        HitBtc = 4,
        Bittrex = 5,
        OkexSpot = 6,
        OkexFutures = 7,
		CryptoFacilities = 9,
        BinanceFutures = 10,       
		Huobi = 11,
		Gmex = 12,
		LBank = 13,
		Gate = 14,
		Bitforex = 15,
		Bitkub = 16,
		UEX = 17,
		Upbit = 18,
		Poloniex = 19,
		Bithumb = 20,
        Deribit = 21,
        BinanceFuturesCoin = 22,
    }

    public enum ExchangeMode
    {
        Trade,
        View,
        PartTrade
    }
}
