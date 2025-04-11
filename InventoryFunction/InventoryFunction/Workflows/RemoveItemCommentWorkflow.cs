using InventoryFunction.Data;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IRemoveItemCommentWorkflow
    {
        Task RemoveItemComment(int id, string lastmodifiedby);
    }

    public class RemoveItemCommentWorkflow : IRemoveItemCommentWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveItemCommentOperations _removeItemCommentOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly IItemCommentDataValidator _itemCommentDataValidator;
        private readonly IItemCommentDeepValidator _deepValidator;

        public RemoveItemCommentWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemCommentWorkflow>();
            _configuration = configuration;
            _removeItemCommentOperations = new RemoveItemCommentOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemCommentDeepValidator(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
        }

        
        public async Task RemoveItemComment(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItemComment request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateItemCommentId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemCommentOperations.RemoveItemComment(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItemComment success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300015] RemoveItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300016] RemoveItemComment Exception: {e}.");
                throw;
            }
        }
    }
}