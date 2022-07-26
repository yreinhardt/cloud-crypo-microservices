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

/*
 POST: /portfolio/createPortfolio
DELETE: /portfolio/deletePortfolio/{portfolioId}
POST: /portfolio/addCoin
DELETE: /portfolio/deleteCoin
GET: getPortfolio/{portfolioId}
 */

app.MapPost("/portfolio/createPortfolio", async (Portfolio folio, PortfolioDbContext db) =>
{
    
    if (folio.Assets.Count % 2 != 0)
    {
        return Results.BadRequest("Provide correct Assetinformation. Each Asset must have a Name and Price");
    }

    folio.CreatedOn = folio.CreatedOn.ToUniversalTime();

    await db.AddAsync(folio);
    await db.SaveChangesAsync();

    return Results.Ok(folio);
});


app.MapGet("/portfolio/getPortfolio/{portfolioName}/{userId}", async (string portfolioName, Guid userId, PortfolioDbContext db) =>
{
    // filter for match between userid and portfolioname
    var results = await db.Portfolio
        .Where(x => (x.PortfolioName == portfolioName) && (x.UserId == userId))
        .ToListAsync();

    return Results.Ok(results);
});

app.Run();

// listen to port http:4000, https:4001