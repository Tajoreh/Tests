using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApi.Entities;
using SampleApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<DatabaseContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(
        connectionString,
        options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddScoped<ICompanyRepository,CompanyRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("{companyId:int}", async (int companyId, ICompanyRepository repository) =>
{
   var company = await repository.Get(companyId);

    if (company is null)
        return Results.NotFound($"The company with id '{companyId}' was not found.");

    return Results.Ok(company);
});

app.MapPost("create", async ([FromBody] Company company, ICompanyRepository repository) =>
{
    await repository.Create(company);

    return Results.Ok(company);
});

app.Run();
