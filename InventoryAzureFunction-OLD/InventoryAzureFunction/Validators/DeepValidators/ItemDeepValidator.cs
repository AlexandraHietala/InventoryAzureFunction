using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SEInventoryCollection.Models.Classes;
using SEInventoryCollection.Models.System;
using SEInventoryCollection.Validators.DataValidators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEInventoryCollection.Validators.DeepValidators
{
    public interface IItemDeepValidator
    {
        Task<string> ValidateAddItem(Item item);
        Task<string> ValidateUpdateItem(Item item);
        Task<string> ValidateItemId(int id);
        Task<string> ValidateCollectionId(int id);
    }

    public class ItemDeepValidator : IItemDeepValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly ISeriesDataValidator _seriesDataValidator;
        private readonly IBrandDataValidator _brandDataValidator;
        private readonly ICollectionDataValidator _collectionDataValidator;

        public ItemDeepValidator(ILoggerFactory loggerFactory, IConfiguration configuration, IItemDataValidator itemDataValidator, ISeriesDataValidator seriesDataValidator, IBrandDataValidator brandDataValidator, ICollectionDataValidator collectionDataValidator)
        {
            _logger = loggerFactory.CreateLogger<ItemDeepValidator>();
            _configuration = configuration;
            _itemDataValidator = itemDataValidator;
            _seriesDataValidator = seriesDataValidator;
            _brandDataValidator = brandDataValidator;
            _collectionDataValidator = collectionDataValidator;
        }

        public async Task<string> ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400008, Message = "Item object is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId.HasValue)
            {
                bool seriesExists = await ValidateSeriesExists(item.SeriesId.Value);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400009, Message = "Series does not exist." });
            }

            if (item != null && item.BrandId != null && item.BrandId.HasValue)
            {
                bool brandExists = await ValidateBrandExists(item.BrandId.Value);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 200400010, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400011, Message = "Item object is invalid." });

            if (item != null)
            {
                bool itemExists = await ValidateItemExists(item.Id);
                if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400012, Message = "Item does not exist." });
            }

            if (item != null && item.SeriesId != null && item.SeriesId.HasValue)
            {
                bool seriesExists = await ValidateSeriesExists(item.SeriesId.Value);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400013, Message = "Series does not exist." });
            }

            if (item != null && item.BrandId != null && item.BrandId.HasValue)
            {
                bool brandExists = await ValidateBrandExists(item.BrandId.Value);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 200400014, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateItemId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateItemExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400015, Message = "Item does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateCollectionId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateCollectionExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400016, Message = "Collection does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateItemExists(int id)
        {
            bool valid = await _itemDataValidator.VerifyItem(id);
            return valid;
        }

        private async Task<bool> ValidateSeriesExists(int id)
        {
            bool valid = await _seriesDataValidator.VerifySeries(id);
            return valid;
        }

        private async Task<bool> ValidateBrandExists(int id)
        {
            bool valid = await _brandDataValidator.VerifyBrand(id);
            return valid;
        }

        private async Task<bool> ValidateCollectionExists(int id)
        {
            bool valid = await _collectionDataValidator.VerifyCollection(id);
            return valid;
        }
    }
}
