namespace Microservice.Portfolio.DataAccess;

public class Portfolio
{
    public Guid PortfolioId { get; set; }
    public string PortfolioName { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid UserId { get; set; }
    // assets contain price and name
    public IEnumerable<Asset> Coin { get; set; }

    public class Asset
    {
        public Guid AssetId { get; set; }
        public string AssetName { get; set; }
        public float AssetPrice { get; set; }
    }

}

