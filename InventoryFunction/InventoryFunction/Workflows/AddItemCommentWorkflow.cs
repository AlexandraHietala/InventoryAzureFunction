using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IAddItemCommentWorkflow
    {
        Task<int> AddItemComment(ItemComment comment);
    }

    public class AddItemCommentWorkflow : IAddItemCommentWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddItemCommentOperations _addItemCommentOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly IItemCommentDataValidator _itemCommentDataValidator;
        private readonly IItemCommentDeepValidator _deepValidator;


        public AddItemCommentWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemCommentWorkflow>();
            _configuration = configuration;
            _addItemCommentOperations = new AddItemCommentOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemCommentDeepValidator(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
        }

        public async Task<int> AddItemComment(ItemComment comment)
        {
            _logger.LogDebug("AddItemComment request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateAddItemComment(comment);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemCommentDto commentDto = ItemCommentConverter.ConvertItemCommentToItemCommentDto(comment);
                int newCommentId = await _addItemCommentOperations.AddItemComment(commentDto);

                // Respond
                _logger.LogInformation("AddItemComment success response.");
                return newCommentId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300001] AddItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300002] AddItemComment Exception: {e}.");
                throw;
            }
        }
    }
}