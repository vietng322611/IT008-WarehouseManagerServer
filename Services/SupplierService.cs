using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class SupplierService(ISupplierRepository supplierRepository) : ISupplierService
{
    public Task<List<Supplier>> GetByWarehouseAsync(int warehouseId)
        => supplierRepository.GetByWarehouseAsync(warehouseId);

    public Task<Supplier?> GetByKeyAsync(int supplierId)
        => supplierRepository.GetByKeyAsync(supplierId);

    public Task<Supplier> AddAsync(Supplier supplier)
        => supplierRepository.AddAsync(supplier);

    public Task<Supplier?> UpdateAsync(Supplier supplier)
        => supplierRepository.UpdateAsync(supplier);

    public Task UpsertAsync(List<Supplier> suppliers)
        => supplierRepository.UpsertAsync(suppliers);

    public Task<bool> DeleteAsync(int supplierId)
        => supplierRepository.DeleteAsync(supplierId);
}