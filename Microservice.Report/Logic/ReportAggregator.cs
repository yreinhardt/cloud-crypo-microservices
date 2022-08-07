using Newtonsoft.Json;
using System.Linq;
using Microservice.Report.DataAccess;
using Microservice.Report.Models;
using Microsoft.Extensions.Options;

namespace Microservice.Report.Logic;

// data is saved to own postgres instance
public class PortfolioReportAggregator : IPortfolioReportAggregator
{
    private readonly IHttpClientFactory _http; // create instances of http clients to make requests
    private readonly ILogger<PortfolioReportAggregator> _logger;
    private readonly ReportDbContext _db; // interact with own postgres db

    // constructor
    public PortfolioReportAggregator(
        IHttpClientFactory http,
        ILogger<PortfolioReportAggregator> logger,
        ReportDbContext db
    )
    {
        _http = http;
        _logger = logger;
        _db = db;

    }


    public async Task<PortfolioReport> BuildReport(string portfolioName, Guid userId)
    {
        var portfolioClient = _http.CreateClient("portfolioDb");

        var portfolioData = await FetchPortfolioData(portfolioClient, portfolioName, userId);

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

        var coinClient = _http.CreateClient("coin");

        var coinData = await FetchCoinData(coinClient);


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


    // construct portfolio endpoint 
    private string BuildPortfolioEndpoint(string portfolioName, Guid userId)
    {
        return $"getPortfolio/{portfolioName}/{userId}";

    }


    private async Task<List<CoinModel>> FetchCoinData(HttpClient http)
    {
        // build coin endpoint
        //string coinDataEndpoint = BuildCoinEndpoint(assets);

        // TODO correct deserialization
        var coinRecords = await http.GetAsync("simple/price?ids=bitcoin%2Cethereum%2Ccardano&vs_currencies=usd%2Ceur");
        //var json = await coinRecords.Content.ReadAsStringAsync();
        //var res = JsonConvert.DeserializeObject<CoinModel>(json);
        //var coinData = JsonConvert.DeserializeObject<CoinModel>(json);
        var coinData = await coinRecords.Content.ReadFromJsonAsync<CoinModel>();

        return null;

    }


    // build coin endpoint based on parameter assets
    private string BuildCoinEndpoint(List<string> assets)
    {
        throw new NotImplementedException();
    }
}


