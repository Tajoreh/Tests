using SampleApi.Entities;

namespace SampleApi.Repositories;

public interface ICompanyRepository
{
    Task Create(Company company);

    Task<Company?> Get(int id);

}