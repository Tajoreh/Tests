using Microsoft.EntityFrameworkCore;
using SampleApi.Entities;

namespace SampleApi.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private DatabaseContext dbContext;

    public CompanyRepository(DatabaseContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Create(Company company)
    {
        await dbContext
            .Set<Company>()
            .AddAsync(company);

        await dbContext.SaveChangesAsync();
    }

    public async Task<Company?> Get(int companyId) =>
        await dbContext
            .Set<Company>()
            .AsSplitQuery()
            .Include(x => x.Employees)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyId);

}