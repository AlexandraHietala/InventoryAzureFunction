using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IGetRoleWorkflow
    {
        Task<Role> GetRole(int id);
        Task<List<Role>> GetRoles();
    }

    public class GetRoleWorkflow : IGetRoleWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRoleOperations _roleOperations;
        private readonly IUserDataValidator _userDataValidator;
        private readonly IRoleDataValidator _roleDataValidator;
        private readonly IUserDeepValidator _deepValidator;

        public GetRoleWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleWorkflow>();
            _configuration = configuration;
            _roleOperations = new GetRoleOperations(loggerFactory, configuration);
            _userDataValidator = new UserDataValidator(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidator(loggerFactory, configuration);
            _deepValidator = new UserDeepValidator(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task<Role> GetRole(int id)
        {
            _logger.LogDebug("GetRole request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateRoleId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                RoleDto roleDto = await _roleOperations.GetRole(id);
                Role role = RoleConverter.ConvertRoleDtoToRole(roleDto);

                // Respond
                _logger.LogInformation("GetRole success response.");
                return role;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300005] GetRole ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300006] GetRole Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Role>> GetRoles()
        {
            _logger.LogDebug("GetRoles request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<RoleDto> roleDtos = await _roleOperations.GetRoles();
                List<Role> roles = RoleConverter.ConvertListRoleDtosToListRoles(roleDtos);

                // Respond
                _logger.LogInformation("GetRoles success response.");
                return roles;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300007] GetRoles ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300008] GetRoles Exception: {e}.");
                throw;
            }
        }
    }
}