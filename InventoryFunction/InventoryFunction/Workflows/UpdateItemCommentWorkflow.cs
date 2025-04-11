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
    public interface IUpdateItemCommentWorkflow
    {
        Task UpdateItemComment(ItemComment comment);
    }

    public class UpdateItemCommentWorkflow : IUpdateItemCommentWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateItemCommentOperations _updateItemCommentOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly IItemCommentDataValidator _itemCommentDataValidator;
        private readonly IItemCommentDeepValidator _deepValidator;

        public UpdateItemCommentWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentWorkflow>();
            _configuration = configuration;
            _updateItemCommentOperations = new UpdateItemCommentOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemCommentDeepValidator(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
        }

        public async Task UpdateItemComment(ItemComment comment)
        {
            _logger.LogDebug("UpdateItemComment request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUpdateItemComment(comment);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemCommentDto commentDto = ItemCommentConverter.ConvertItemCommentToItemCommentDto(comment);
                await _updateItemCommentOperations.UpdateItemComment(commentDto);

                // Respond
                _logger.LogInformation("UpdateItemComment success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300019] UpdateItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300020] UpdateItemComment Exception: {e}.");
                throw;
            }
        }
    }
}