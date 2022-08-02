namespace Microservice.Report.Models;

public class PortfolioModel
{
    public Guid PortfolioId { get; set; }
    public Guid UserId { get; set; }
    // contains name, price, count
    public List<string> AssetList { get; set; }


}

