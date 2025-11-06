using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetByWarehouseAsync(int warehouseId);
    Task<Supplier?> GetByKeyAsync(int supplierId);
    Task<Supplier> AddAsync(Supplier supplier);
    Task<Supplier?> UpdateAsync(Supplier supplier);
    Task<bool> DeleteAsync(int supplierId);
}