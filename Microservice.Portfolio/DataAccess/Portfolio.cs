namespace Microservice.Portfolio.DataAccess;

public class Portfolio
{
    public Guid PortfolioId { get; set; }
    public string PortfolioName { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid UserId { get; set; }
    // assets are modelled as list of strings
    // ["bitcoin", "22.100", "etherium", "1.200"]
    public List<string> Assets { get; set; }


    /*
    // assets contain price and name
    [ForeignKey("Asset")]
    public List<Asset> Coin { get; set; }

    public class Asset
    {
        public Guid AssetId { get; set; }
        public string CoinId { get; set; }
        public float CoinPrice { get; set; }
    }*/

}

