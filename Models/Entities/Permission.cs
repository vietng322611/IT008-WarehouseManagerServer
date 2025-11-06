using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class Permission
{
    public int UserId { get; set; }

    public int WarehouseId { get; set; }

    public ICollection<PermissionEnum> Permissions { get; set; }
}