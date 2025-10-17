using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface ISupplierService
{
    Task<List<Supplier>> GetAllAsync();
    Task<Supplier?> GetByKeyAsync(int supplierId);
    Task<Supplier> AddAsync(Supplier supplier);
    Task<Supplier?> UpdateAsync(Supplier supplier);
    Task<bool> DeleteAsync(int supplierId);
}