using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IUpdateUserWorkflow
    {
        Task UpdateUser(User user);
    }

    public class UpdateUserWorkflow : IUpdateUserWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateUserOperations _updateUserOperations;
        private readonly IUserDataValidator _userDataValidator;
        private readonly IRoleDataValidator _roleDataValidator;
        private readonly IUserDeepValidator _deepValidator;

        public UpdateUserWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateUserWorkflow>();
            _configuration = configuration;
            _updateUserOperations = new UpdateUserOperations(loggerFactory, configuration);
            _userDataValidator = new UserDataValidator(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidator(loggerFactory, configuration);
            _deepValidator = new UserDeepValidator(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task UpdateUser(User user)
        {
            _logger.LogDebug("UpdateUser request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUpdate(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                UserDto userDto = UserConverter.ConvertUserToUserDto(user);
                await _updateUserOperations.UpdateUser(userDto);

                // Respond
                _logger.LogInformation("UpdateUser success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300015] UpdateUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300016] UpdateUser Exception: {e}.");
                throw;
            }
        }
    }
}