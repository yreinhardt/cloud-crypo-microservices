using Newtonsoft.Json;

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


    // buildreport based on user and portfolio
    public async Task<PortfolioReport> BuildReport(string portfolioName, Guid userId)
    {
        var httpClient = _http.CreateClient();

        var portfolioData = await FetchPortfolioData(httpClient, portfolioName, userId);

        // get coinData based on portfolioData (multiple requests)
        // var coinData = await FetchCoinData(httpClient);


        var report = new PortfolioReport
        {

            CreatedOn = DateTime.UtcNow,

        };

        return report;
    }

    /*
     * To not referencing Portfolio or Coin models directly new models are needed
     * No dependency between each service (following microservice architecture)
     */

    // fetch data from portfolio service
    private async Task<List<PortfolioModel>> FetchPortfolioData(HttpClient httpClient, string portfolioName, Guid userId)
    {
        // build endpoint
        var endpoint = BuildPortfolioEndpoint(portfolioName, userId);

        // portfolio records based on constructed endpoint
        var portfolioRecords = await httpClient.GetAsync(endpoint);

        var portfolioData = await portfolioRecords.Content.ReadFromJsonAsync<List<PortfolioModel>>();

        /*
        Console.WriteLine($"Date: {portfolioData?.userId}");
        Console.WriteLine($"TemperatureCelsius: {weatherForecast?.portfolioID}");
        Console.WriteLine($"Summary: {weatherForecast?.portfolioName}");
        */

        // TODO actuall empty return

        return portfolioData ?? new List<PortfolioModel>();

    }

    // construct portfolio endpoint getPortfolio based on ReportDataConfig
    private string BuildPortfolioEndpoint(string portfolioName, Guid userId)
    {
        var folioServiceProtocol = _reportDataConfig.PortfolioDataProtocol;
        var folioServiceHost = _reportDataConfig.PortfolioDataHost;
        var folioServicePort = _reportDataConfig.PortfolioDataPort;

        // return endpoint
        return $"{folioServiceProtocol}://{folioServiceHost}:{folioServicePort}/portfolio/getPortfolio/{portfolioName}/{userId}";

    }

    // fetch data from coin service
    private async Task<List<CoinModel>> FetchCoinData(HttpClient httpClient)
    {
        // TODO parallel requests
        throw new NotImplementedException();
    }


}


