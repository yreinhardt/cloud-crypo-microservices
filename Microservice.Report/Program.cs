using Microservice.Report.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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


app.Run();

// listen to port http:5000, https:5001