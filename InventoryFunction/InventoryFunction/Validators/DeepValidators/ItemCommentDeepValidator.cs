using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.System;
using InventoryFunction.Validators.DataValidators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryFunction.Validators.DeepValidators
{
    public interface IItemCommentDeepValidator
    {
        Task<string> ValidateAddItemComment(ItemComment comment);
        Task<string> ValidateUpdateItemComment(ItemComment comment);
        Task<string> ValidateItemCommentId(int id);
        Task<string> ValidateItemId(int itemId);
    }

    public class ItemCommentDeepValidator : IItemCommentDeepValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly IItemCommentDataValidator _commentDataValidator;

        public ItemCommentDeepValidator(ILoggerFactory loggerFactory, IConfiguration configuration, IItemDataValidator itemDataValidator, IItemCommentDataValidator commentDataValidator)
        {
            _logger = loggerFactory.CreateLogger<ItemCommentDeepValidator>();
            _configuration = configuration;
            _itemDataValidator = itemDataValidator;
            _commentDataValidator = commentDataValidator;
        }

        public async Task<string> ValidateAddItemComment(ItemComment comment)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (comment == null)
                failureList.Add(new ValidationFailure() { Code = 200400001, Message = "Comment object is invalid." });

            if (comment != null)
            {
                bool itemExists = await ValidateItemExists(comment.ItemId);
                if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400002, Message = "Item does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateItemComment(ItemComment comment)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (comment == null)
                failureList.Add(new ValidationFailure() { Code = 200400003, Message = "Comment object is invalid." });

            if (comment != null)
            {
                bool itemExists = await ValidateItemExists(comment.ItemId);
                if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400004, Message = "Item does not exist." });
            }

            if (comment != null)
            {
                bool itemExists = await ValidateItemCommentExists(comment.Id);
                if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400005, Message = "Comment does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateItemCommentId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateItemCommentExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400006, Message = "Comment does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateItemId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateItemExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400007, Message = "Item does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateItemExists(int id)
        {
            bool valid = await _itemDataValidator.VerifyItem(id);
            return valid;
        }

        private async Task<bool> ValidateItemCommentExists(int id)
        {
            bool valid = await _commentDataValidator.VerifyItemComment(id);
            return valid;
        }
    }
}
