

using InventoryFunction.Models.Classes;
using InventoryFunction.Models.DTOs;

namespace InventoryFunction.Models.Converters
{
    public class AuthConverter
    {
        public static Auth ConvertAuthDtoToAuth(AuthDto source)
        {
            return new Auth()
            {
                PassSalt = source.PASS_SALT,
                PassHash = source.PASS_HASH,
                RoleId = source.ROLE_ID != null ? source.ROLE_ID : null
            };
        }

        public static AuthDto ConvertAuthToAuthDto(Auth source)
        {
            return new AuthDto()
            {
                PASS_SALT = source.PassSalt,
                PASS_HASH = source.PassHash,
                ROLE_ID = source.RoleId != null ? source.RoleId : null
            };
        }

    }
}
