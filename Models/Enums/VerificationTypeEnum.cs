using NpgsqlTypes;

namespace WarehouseManagerServer.Models.Enums;

public enum VerificationTypeEnum
{
    Register,
    [PgName("change_password")]
    ChangePassword,
    Recovery,
}