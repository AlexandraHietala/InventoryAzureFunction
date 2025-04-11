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
    public interface IGetAuthWorkflow
    {
        Task<Auth> GetAuth(int id);
    }

    public class GetAuthWorkflow : IGetAuthWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetAuthOperations _authOperations;
        private readonly IUserDataValidator _userDataValidator;
        private readonly IRoleDataValidator _roleDataValidator;
        private readonly IUserDeepValidator _deepValidator;

        public GetAuthWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetAuthWorkflow>();
            _configuration = configuration;
            _authOperations = new GetAuthOperations(loggerFactory, configuration);
            _userDataValidator = new UserDataValidator(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidator(loggerFactory, configuration);
            _deepValidator = new UserDeepValidator(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task<Auth> GetAuth(int id)
        {
            _logger.LogDebug("GetAuth request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                AuthDto authDto = await _authOperations.GetAuth(id);
                Auth auth = AuthConverter.ConvertAuthDtoToAuth(authDto);

                // Respond
                _logger.LogInformation("GetAuth success response.");
                return auth;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300003] GetAuth ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300004] GetAuth Exception: {e}.");
                throw;
            }
        }

    }
}