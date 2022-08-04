namespace Microservice.Report.Config;

// configure the location/endpoint to communicate with coin and portfolio service
public class ReportDataConfig
{
    // config protocol (http,https), host and port of specific service
    public string PortfolioDataProtocol { get; set; } = string.Empty;
    public string PortfolioDataHost { get; set; } = string.Empty;
    public string PortfolioDataPort { get; set; } = string.Empty;

    public string CoinDataProtocol { get; set; } = string.Empty;
    public string CoinDataHost { get; set; } = string.Empty;

}


