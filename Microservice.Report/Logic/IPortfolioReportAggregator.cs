using Microservice.Report.DataAccess;

namespace Microservice.Report.Logic;

// build a report based on coin and portfolio services (aggregate data), persist data to db
public interface IPortfolioReportAggregator
{
    public Task<PortfolioReport> BuildReport(string portfolioName, Guid userId);

}

