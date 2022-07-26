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
    folio.CreatedOn = folio.CreatedOn.ToUniversalTime();

    await db.AddAsync(folio);
    await db.SaveChangesAsync();

    return Results.Ok(folio);
});


app.MapGet("/portfolio/getPortfolio/{portfolioName}", async (string portfolioName, PortfolioDbContext db) =>
{
    // TODO: Add filter where userid matches current login user
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