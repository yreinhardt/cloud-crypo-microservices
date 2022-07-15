var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/portfolio/getPortfolio/{portfolioId}", (int portfolioId) =>
{
    return Results.Ok(portfolioId);
});

app.Run();

// listen to port http:4000, https:4001