namespace Microservice.Report.DataAccess;

public class Report
{
    public Guid ReportId { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid PortfolioId { get; set; }
    public Guid UserId { get; set; }
    public string Portfolioname { get; set; }
    public float WinLossRatioAbsolute { get; set; }
    public float WinLossRatioPercentage { get; set; }
    public int AssetCount { get; set; }
    public string HighestPerformanceAsset { get; set; }
    public string LowestPerformanceAsset { get; set; }
    // Asset specific performance, index 0 of AssetList
    // corresponds to performance of AssetPerformance with index 0
    public List<string> AssetList { get; set; }
    public List<float> AssetPerformanceAbsolute { get; set; }
    public List<float> AssetPerformancePercentage { get; set; }
}
