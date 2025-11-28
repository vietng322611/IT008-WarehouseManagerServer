using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface ISupplierService
{
    Task<List<Supplier>> GetByWarehouseAsync(int warehouseId);
    Task<Supplier?> GetByKeyAsync(int supplierId);
    Task<Supplier> AddAsync(SupplierDto supplier);
    Task<Supplier?> UpdateAsync(SupplierDto supplier);
    Task UpsertAsync(List<SupplierDto> suppliers);
    Task<bool> DeleteAsync(int supplierId);
}