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
PUT: /portfolio/addCoin
PUT: /portfolio/deleteCoin
 */

// create portfolio
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

// get user specific portfolio by name
app.MapGet("/portfolio/getPortfolio/{portfolioName}/{userId}", async (string portfolioName, Guid userId, PortfolioDbContext db) =>
{
    // filter for match between userid and portfolioname
    var results = await db.Portfolio
        .Where(x => (x.PortfolioName == portfolioName) && (x.UserId == userId))
        .ToListAsync();

    return Results.Ok(results);
});

// delete portfolio by id
app.MapDelete("/portfolio/deletePortfolio/{portfolioId}", async (Guid portfolioId, PortfolioDbContext db) =>
{

    if (await db.Portfolio.FindAsync(portfolioId) is Portfolio folio)
    {

        db.Portfolio.Remove(folio);

        await db.SaveChangesAsync();

        return Results.NoContent();

        // return Results.Ok(folio);
    }

    return Results.NotFound();

});

// add assets from portfolio with id, request body = folioUpdate (only id and assets needed)
app.MapPut("/portfolio/addAsset", async (Portfolio folioUpdate, PortfolioDbContext db) =>
{

    var record = await db.Portfolio.FindAsync(folioUpdate.PortfolioId);

    if (record is null)
    {
        return Results.NotFound("Provide valid Portfolio Id.");
    }

 
    // check if asset is already in portfolio
    // at this moment possible to add only one asset
    // TODO fix multiple assets
    var match = record.Assets
        .Where(x => x.Contains(folioUpdate.Assets[0]));

    if (match != null)
    {
        // asset name
        record.Assets.Add(folioUpdate.Assets[0]);
        await db.SaveChangesAsync();
        // asset price
        record.Assets.Add(folioUpdate.Assets[1]);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.BadRequest("Portfolio contains Asset already");



});


app.Run();

// listen to port http:4000, https:4001