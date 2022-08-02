using Microservice.Report.Config;
using Microservice.Report.DataAccess;
using Microservice.Report.Models;
using Microsoft.Extensions.Options;

namespace Microservice.Report.Logic;

// data is saved to own postgres instance
public class PortfolioReportAggregator : IPortfolioReportAggregator
{
    private readonly IHttpClientFactory _http; // create instances of http clients to make requests
    private readonly ILogger<PortfolioReportAggregator> _logger;
    private readonly ReportDataConfig _reportDataConfig; // configuration
    private readonly ReportDbContext _db; // interact with own postgres db

    // constructor
    public PortfolioReportAggregator(
        IHttpClientFactory http,
        ILogger<PortfolioReportAggregator> logger,
        IOptions<ReportDataConfig> reportDataConfig, // options pattern to map config and inject into class
        ReportDbContext db
    )
    {
        _http = http;
        _logger = logger;
        _reportDataConfig = reportDataConfig.Value;
        _db = db;

    }

    // buildreport from interface
    public async Task<PortfolioReport> BuildReport()
    {
        var httpClient = _http.CreateClient();
        var coinData = await FetchCoinData(httpClient);
        var PortfolioData = await FetchPortfolioData(httpClient);

    }

    /*
     * To not referencing Portfolio or Coin models directly new models are needed
     * No dependency between each service (following microservice architecture)
     */

    // fetch data from portfolio service
    private async Task<List<PortfolioModel>> FetchPortfolioData(HttpClient httpClient)
    {
        throw new NotImplementedException();
    }

    // fetch data from coin service
    private async Task<List<CoinModel>> FetchCoinData(HttpClient httpClient)
    {
        throw new NotImplementedException();
    }
}


