using System.Text.RegularExpressions;
using Microservice.Coin.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

/* 
 Register HttpClient Object explicit in Dependency Injection Service
 because only one API (CoinGecko) will be used
*/

builder.Services
    .AddScoped(hc => new HttpClient { BaseAddress = new Uri("https://api.coingecko.com/api/v3/") });


var app = builder.Build();
app.MapGet("/coin/testlocal", async () =>
{
    
    return Results.Ok("test");
});



// passing HttpClient object as input parameter to delegate handler
app.MapGet("/coin/healthcheck", async (HttpClient http) =>
{
    // return HttpResponseMessage
    var res = await http.GetAsync("ping");

    if (!res.IsSuccessStatusCode)
    {
        return Results.NotFound();
    }

    var json = await res.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<Healthcheck>(json);

    return Results.Ok(responseObject);
});

// get trending coins from last 24h
app.MapGet("/coin/trending", async (HttpClient http) =>
{

    var res = await http.GetAsync("search/trending");

    if (!res.IsSuccessStatusCode)
    {
        return Results.NotFound();
    }

    //var response = await http.GetFromJsonAsync<TrendingCoins>("search/trending");
    var json = await res.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<TrendingCoins>(json);

    return Results.Ok(responseObject);
});

// get historic data from coin
app.MapGet("/coin/historic/{coin}/{date}", async (string coin, string date, HttpClient http) =>
{

    // coin = lowercase
    coin = coin.ToLower();

    // check dateformat with regex 
    Regex r = new Regex(@"\d{2}-\d{2}-\d{4}");
    if (!r.IsMatch(date))
    {
        return Results.BadRequest("Provide correct dateformat: dd-mm-yyyy");
    }

    var res = await http.GetAsync($"coins/{coin}/history?date={date}&localization=false");

    if (!res.IsSuccessStatusCode)
    {
        return Results.NotFound();
    }

    var json = await res.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<HistoricalData>(json);
    
    return Results.Ok(responseObject);


});


// get current price in usd, eur from coin (e.g. bitcoin)
app.MapGet("/coin/currentprice/{coin}", async (string coin, HttpClient http) =>
{
    // coin = lowercase
    coin = coin.ToLower();

    var res = await http.GetAsync($"coins/{coin}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false");

    if (!res.IsSuccessStatusCode)
    {
        return Results.NotFound();
    }

    var json = await res.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<CurrentCoin>(json);

    //return Results.Ok(responseObject);
    return Results.Ok(responseObject);

});


app.Run();


// listen to port http:3000, https:3001