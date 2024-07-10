using Microsoft.EntityFrameworkCore;
using Minimal.Api.Model;

namespace Minimal.Api.Data;

public class ProductRepo : IRepo
{
    private readonly ProductDbContext _context;

    public ProductRepo(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task<Product> GetBydId(int id) => (await _context.Products.FindAsync(id))!;

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync(); 
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(); 
    }
}