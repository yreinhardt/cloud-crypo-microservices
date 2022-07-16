using Microservice.Portfolio.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortfolioDbContext>(
    opts =>
    {
        opts.EnableSensitiveDataLogging();
        opts.EnableDetailedErrors();
        opts.UseNpgsql(builder.Configuration.GetConnectionString("ServiceDb"));
    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet("/portfolio/getPortfolio/{portfolioName}", async (string portfolioName, PortfolioDbContext db) =>
{
    var results = await db.Portfolio
        .Where(x => x.PortfolioName == portfolioName)
        .ToListAsync();

    if (results == null)
    {
        return Results.BadRequest("Provide correct Portfolioname");
    }
    return Results.Ok(results);
});

app.Run();

// listen to port http:4000, https:4001