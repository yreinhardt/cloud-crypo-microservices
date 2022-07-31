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
    // validate req body => must contain 3 elements (name, price, count)
    if (folio.Assets.Count % 3 != 0)
    {
        return Results.BadRequest("Provide correct Assetinformation. Each Asset must have a Name, Price and Count");
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

    // validation if records are empty
    if (record is null)
    {
        return Results.NotFound("Provide valid Portfolio Identifier.");
    }

    // validate req body => must contain 3 elements (name, price, count)
    if (folioUpdate.Assets.Count % 3 != 0)
    {
        return Results.BadRequest("Provide correct Assetinformation. Each Asset must have a Name, Price and Count");
    }

    // get asset names from list to check for duplicates
    // every third element equals asset name
    var assetNames = folioUpdate.Assets.Where((x, i) => i % 3 == 0).ToList();

    List<string> duplicates = record.Assets.Intersect(assetNames).ToList();
    // validation check for duplicates
    if (duplicates.Any())
    {
        return Results.BadRequest("Can not add already existing Asset");
    }

    record.Assets.AddRange(folioUpdate.Assets);

    await db.SaveChangesAsync();

    return Results.NoContent();

});

// delete asset from portfolio based on id
app.MapPut("/portfolio/deleteAsset", async (Portfolio folioUpdate, PortfolioDbContext db) =>
{
    var record = await db.Portfolio.FindAsync(folioUpdate.PortfolioId);

    if (record is null)
    {
        return Results.NotFound("Provide valid Portfolio Identifier.");
    }

    // check if multiple assets need to be deleted
    if (folioUpdate.Assets.Count > 0)
    {
        // get indices of assets to delete
        List<int> indices = new List<int>();

        for (int i = 0; i < folioUpdate.Assets.Count; i++)
        {
            // asset name
            indices.Add(record.Assets.IndexOf(folioUpdate.Assets[i]));
            // asset price
            indices.Add(record.Assets.IndexOf(folioUpdate.Assets[i])+1);
            // number of purchased assets (count)
            indices.Add(record.Assets.IndexOf(folioUpdate.Assets[i])+2);
        }

        var recordCopy = record.Assets;

        // remove asset by given index. remove from last to first elements to avoid changing indices
        for (int i = indices.Count - 1; i >= 0; i--)
        {
            recordCopy.RemoveAt(i);
        }

        record.Assets = recordCopy;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }


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