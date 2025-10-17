using System.Linq.Expressions;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class ProductService(IProductRepository productRepository): IProductService
{
    public Task<List<Product>> GetAllAsync()
        => productRepository.GetAllAsync();
    public Task<Product?> GetByKeyAsync(int productId)
        => productRepository.GetByKeyAsync(productId);
    public Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters)
        => productRepository.FilterAsync(filters);
    public Task<Product> AddAsync(Product product)
        => productRepository.AddAsync(product);
    public Task<Product?> UpdateAsync(Product product)
        => productRepository.UpdateAsync(product);
    public Task<bool> DeleteAsync(int productId)
        => productRepository.DeleteAsync(productId);
}