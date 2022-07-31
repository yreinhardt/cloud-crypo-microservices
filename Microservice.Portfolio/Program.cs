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

// add asset from portfolio with id, request body = folioUpdate (only id and assets needed)
app.MapPut("/portfolio/addAsset", async (Portfolio folioUpdate, PortfolioDbContext db) =>
{

    var record = await db.Portfolio.FindAsync(folioUpdate.PortfolioId);

    if (record is null)
    {
        return Results.NotFound("Provide valid Portfolio Identifier.");
    }

 
    // check if asset is already in portfolio
    // at this moment possible to add only one asset
    // TODO fix for multiple assets
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

// delete asset from portfolio based on id
app.MapPut("/portfolio/deleteAsset", async (Portfolio folioUpdate, PortfolioDbContext db) =>
{
    var record = await db.Portfolio.FindAsync(folioUpdate.PortfolioId);

    if (record is null)
    {
        return Results.NotFound("Provide valid Portfolio Identifier.");
    }

    // at this moment possible to delete only one asset
    // TODO fix for multiple assets

    // find index of asset portfolio
    int indexOfValue = record.Assets.FindIndex(x => x.Contains(folioUpdate.Assets[0]));

    if (indexOfValue == -1)
    {
        return Results.NotFound("Asset not found");
    }

    // copy list and update original list from db
    List<string> updateList = record.Assets;

    // remove purchase price
    updateList.RemoveAt(indexOfValue + 1);

    // remove asset name
    updateList.RemoveAt(indexOfValue);
    
    record.Assets = updateList;

    await db.SaveChangesAsync();

    return Results.NoContent();
    
});


app.Run();

// listen to port http:4000, https:4001