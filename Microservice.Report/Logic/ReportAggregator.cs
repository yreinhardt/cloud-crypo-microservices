using Newtonsoft.Json;
using System.Linq;
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

        // convert string ids to guid
        Guid portfolioId = new Guid(portfolioData[0].portfolioId);
        Guid userSpecificId = new Guid(portfolioData[0].userId);

        // divide by 3 because list contains of name, price, assetcount
        int assetCount = portfolioData[0].assets.Count / 3;

        DateTime actualTime = DateTime.UtcNow;
        string folioName = portfolioData[0].portfolioName;

        List<string> assetList = new List<string>();

        // extract every third element (asset names)
        for (int i = 0; i < portfolioData[0].assets.Count; i += 3)
        {
            assetList.Add(portfolioData[0].assets[i]);
        }

        // build coin endpoint
        string coinDataEndpoint = BuildCoinEndpoint(assetList);

        // TODO add correct multiple client istances for different endpoint
        var coinData = await FetchCoinData(httpClient, coinDataEndpoint);

        // Create report object
        var report = new PortfolioReport
        {
            PortfolioId = portfolioId,
            UserId = userSpecificId,
            CreatedOn = actualTime,
            Portfolioname = folioName,
            WinLossRatioAbsolute = 2,
            WinLossRatioPercentage = 1,
            AssetCount = assetCount,
            HighestPerformanceAsset = "",
            LowestPerformanceAsset = "",
            AssetList = assetList,
            AssetPerformanceAbsolute = null,
            AssetPerformancePercentage = null

        };

        return report;
    }

    private async Task<List<CoinModel>> FetchCoinData(HttpClient httpClient, string endpoint)
    {

        var coinRecords = await httpClient.GetAsync(endpoint);
        var coinData = await coinRecords.Content.ReadFromJsonAsync<List<CoinModel>>();

        Console.WriteLine(coinData);
        Console.WriteLine(coinData[0]);


        return null;

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

    /*
     * // fetch data from coin service
    private async Task<List<CoinModel>> FetchCoinData(HttpClient httpClient, List<string> assets)
    {
        // create list of endpoints, each element contains endpoint url for specific coin
        List<string> endpoints = new List<string>();
        for(int i = 0; i< assets.Count; i++)
        {
            // build coin specific endpoint and add to list
            endpoints.Add(BuildCoinEndpoint(assets[i]));
        }

        // start requests for every url
        var requests = endpoints.Select
            (
                url => httpClient.GetAsync(url)
            ).ToList();

        // await every task to finish
        await Task.WhenAll(requests);

        // get all responses
        var responses = requests.Select
            (
                task => task.Result
            );

        // extract content
        foreach (var r in responses)
        {
            // TODO add correct deserialization, adjust model
            var s = await r.Content.ReadAsStringAsync();
            Console.WriteLine(s);
        }

    }*/

    // build endpont to get current price of coins
    private string BuildCoinEndpoint(List<string> coinList)
    {
        // read specific config values from appsettings.json
        var coinServiceProtocol = _reportDataConfig.CoinDataProtocol;
        var coinServiceHost = _reportDataConfig.CoinDataHost;

        // create needed string ids with correct seperator (e.g.bitcoin%2Cethereum)
        // compare docs: https://www.coingecko.com/en/api/documentation
        string coinUrlInsertion = String.Join("%2", coinList);

        // concatenate final endpoint string
        string endpoint = $"{coinServiceProtocol}://{coinServiceHost}/simple/price?ids=" + coinUrlInsertion + "&vs_currencies=usd%2Ceur";

        return endpoint;

    }
}


