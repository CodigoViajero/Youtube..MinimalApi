using Minimal.Api.Model;

namespace Minimal.Api.Data;

public interface IRepo
{
    Task<List<Product>> GetAllAsync();
    Task<Product> GetBydId(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}