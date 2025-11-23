using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class SupplierService(ISupplierRepository supplierRepository) : ISupplierService
{
    public Task<List<Supplier>> GetByWarehouseAsync(int warehouseId)
    {
        return supplierRepository.GetByWarehouseAsync(warehouseId);
    }

    public Task<Supplier?> GetByKeyAsync(int supplierId)
    {
        return supplierRepository.GetByKeyAsync(supplierId);
    }

    public Task<Supplier> AddAsync(Supplier supplier)
    {
        return supplierRepository.AddAsync(supplier);
    }

    public Task<Supplier?> UpdateAsync(Supplier supplier)
    {
        return supplierRepository.UpdateAsync(supplier);
    }

    public Task<bool> DeleteAsync(int supplierId)
    {
        return supplierRepository.DeleteAsync(supplierId);
    }
}