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
    public interface IGetUserWorkflow
    {
        Task<User> GetUser(int id);
        Task<List<User>> GetUsers();
    }

    public class GetUserWorkflow : IGetUserWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetUserOperations _getUserOperations;
        private readonly IUserDataValidator _userDataValidator;
        private readonly IRoleDataValidator _roleDataValidator;
        private readonly IUserDeepValidator _deepValidator;

        public GetUserWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetUserWorkflow>();
            _configuration = configuration;
            _getUserOperations = new GetUserOperations(loggerFactory, configuration);
            _userDataValidator = new UserDataValidator(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidator(loggerFactory, configuration);
            _deepValidator = new UserDeepValidator(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task<User> GetUser(int id)
        {
            _logger.LogDebug("GetUser request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                UserDto userDto = await _getUserOperations.GetUser(id);
                User user = UserConverter.ConvertUserDtoToUser(userDto);

                // Respond
                _logger.LogInformation("GetUser success response.");
                return user;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300009] GetUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300010] GetUser Exception: {e}.");
                throw;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            _logger.LogDebug("GetUsers request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<UserDto> userDtos = await _getUserOperations.GetUsers();
                List<User> users = UserConverter.ConvertListUserDtoToListUser(userDtos);

                // Respond
                _logger.LogInformation("GetUsers success response.");
                return users;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300011] GetUsers ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300012] GetUsers Exception: {e}.");
                throw;
            }
        }
    }
}