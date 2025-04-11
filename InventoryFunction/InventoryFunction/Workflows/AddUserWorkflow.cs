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
    public interface IAddUserWorkflow
    {
        Task<int> AddUser(User reqUser);
    }

    public class AddUserWorkflow : IAddUserWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddUserOperations _addUserOperations;
        private readonly IUserDataValidator _userDataValidator;
        private readonly IRoleDataValidator _roleDataValidator;
        private readonly IUserDeepValidator _deepValidator;


        public AddUserWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUserWorkflow>();
            _configuration = configuration;
            _addUserOperations = new AddUserOperations(loggerFactory, configuration);
            _userDataValidator = new UserDataValidator(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidator(loggerFactory, configuration);
            _deepValidator = new UserDeepValidator(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task<int> AddUser(User user)
        {
            _logger.LogDebug("AddUser request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateAdd(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                UserDto userDto = UserConverter.ConvertUserToUserDto(user);
                int newUserId = await _addUserOperations.AddUser(userDto);

                // Respond
                _logger.LogInformation("AddUser success response.");
                return newUserId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300001] AddUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300002] AddUser Exception: {e}.");
                throw;
            }
        }
    }
}