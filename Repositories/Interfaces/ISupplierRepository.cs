using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetByWarehouseAsync(int warehouseId);
    Task<Supplier?> GetByKeyAsync(int supplierId);
    Task<Supplier> AddAsync(SupplierDto supplier);
    Task<Supplier?> UpdateAsync(SupplierDto supplier);
    Task UpsertAsync(List<SupplierDto> suppliers);
    Task<bool> DeleteAsync(int supplierId);
}