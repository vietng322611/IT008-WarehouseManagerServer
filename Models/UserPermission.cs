using WarehouseManagerServer.Enums;

namespace WarehouseManagerServer.Models;

public class UserPermission
{
    public int UserId { get; set; }

    public int WarehouseId { get; set; }

    public ICollection<PermissionEnum> PermissionEnum { get; set; }
}