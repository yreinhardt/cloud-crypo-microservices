namespace Microservice.Coin.Models;

public class Fiat
{
    public int usd { get; set; }
    public int eur { get; set; }
}

public class CurrentCoin
{
    public Fiat currentCoin { get; set; }
}



