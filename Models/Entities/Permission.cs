using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class Permission
{
    public int UserId { get; set; }

    public int WarehouseId { get; set; }

    public ICollection<PermissionEnum> Permissions { get; set; } = new List<PermissionEnum>();

    public virtual User User { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}