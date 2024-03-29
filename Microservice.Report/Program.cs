using Microservice.Report.DataAccess;
using Microservice.Report.Logic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// allow injection of httpclient
builder.Services.AddHttpClient("portfolioDb", hc =>
{
    hc.BaseAddress = new Uri("http://localhost:4000/portfolio/");
});
builder.Services.AddHttpClient("coin", hc =>
{
    hc.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
});
// lives for lifetime of method call
builder.Services.AddTransient<IPortfolioReportAggregator, PortfolioReportAggregator>(); 
builder.Services.AddOptions();
builder.Services.AddDbContext<ReportDbContext>(
    opts =>
    {
        opts.EnableSensitiveDataLogging();
        opts.EnableDetailedErrors();
        opts.UseNpgsql(builder.Configuration.GetConnectionString("ServiceDb"));
    }, ServiceLifetime.Transient
);

var app = builder.Build();

/*
 - Report Service makes api requests to coin and portfolio service
 - Wait for data and aggregate to user specific report
 - Searialize to response for client
 */

app.MapGet("/report/{portfolioName}/{userId}", async (
    string portfolioName,
    Guid userId,
    IPortfolioReportAggregator reportAggregator
    ) => {

        // TODO add validation

        var report = await reportAggregator.BuildReport(portfolioName, userId);

        return Results.Ok(report);

    });



app.Run();

// listen to port http:5000, https:5001