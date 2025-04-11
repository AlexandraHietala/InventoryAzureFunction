using InventoryFunction.Data;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IRemoveUserWorkflow
    {
        Task RemoveUser(int id, string lastmodifiedby);
    }

    public class RemoveUserWorkflow : IRemoveUserWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveUserOperations _removeUserOperations;
        private readonly IUserDataValidator _userDataValidator;
        private readonly IRoleDataValidator _roleDataValidator;
        private readonly IUserDeepValidator _deepValidator;

        public RemoveUserWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveUserWorkflow>();
            _configuration = configuration;
            _removeUserOperations = new RemoveUserOperations(loggerFactory, configuration);
            _userDataValidator = new UserDataValidator(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidator(loggerFactory, configuration);
            _deepValidator = new UserDeepValidator(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task RemoveUser(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveUser request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeUserOperations.RemoveUser(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveUser success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300013] RemoveUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300014] RemoveUser Exception: {e}.");
                throw;
            }
        }
    }
}