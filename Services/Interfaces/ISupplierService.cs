using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface ISupplierService
{
    Task<List<Supplier>> GetByWarehouseAsync(int warehouseId);
    Task<Supplier?> GetByKeyAsync(int supplierId);
    Task<Supplier> AddAsync(Supplier supplier);
    Task<Supplier?> UpdateAsync(Supplier supplier);
    Task UpsertAsync(List<Supplier> suppliers);
    Task<bool> DeleteAsync(int supplierId);
}