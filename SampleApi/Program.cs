using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApi.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DatabaseContext>(
optionsBuilder => optionsBuilder.UseSqlServer(connectionString,
options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("{companyId:int}", async (int companyId, DatabaseContext dbContext) =>
{
    var company = await dbContext
        .Set<Company>()
        .AsSplitQuery()
        .Include(x => x.Employees)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == companyId);

    if (company is null)
        return Results.NotFound($"The company with id '{companyId}' was not found.");

    return Results.Ok(company);
});

app.MapPost("create", async ([FromBody] Company company,DatabaseContext dbContext) =>
{
    await dbContext
        .Set<Company>()
        .AddAsync(company);

    await dbContext.SaveChangesAsync();

    return Results.Ok(company);
});

app.Run();
