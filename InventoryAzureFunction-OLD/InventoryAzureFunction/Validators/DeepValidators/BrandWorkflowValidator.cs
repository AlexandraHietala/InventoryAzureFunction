using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SEInventoryCollection.Models.Classes;
using SEInventoryCollection.Models.System;
using SEInventoryCollection.Validators.DataValidators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEInventoryCollection.Validators.DeepValidators
{
    public interface IBrandDeepValidator
    {
        Task<string> ValidateAddBrand(Brand brand);
        Task<string> ValidateUpdateBrand(Brand brand);
        Task<string> ValidateBrandId(int id);
    }

    public class BrandDeepValidator : IBrandDeepValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandDataValidator _dataValidator;

        public BrandDeepValidator(ILoggerFactory loggerFactory, IConfiguration configuration, IBrandDataValidator dataValidator)
        {
            _logger = loggerFactory.CreateLogger<BrandDeepValidator>();
            _configuration = configuration;
            _dataValidator = dataValidator;
        }

        public async Task<string> ValidateAddBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 300400001, Message = "Brand object is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 300400002, Message = "Brand object is invalid." });

            if (brand != null)
            {
                bool brandExists = await ValidateBrandExists(brand.Id);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 300400011, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateBrandId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool brandExists = await ValidateBrandExists(id);
            if (!brandExists) failureList.Add(new ValidationFailure() { Code = 300400003, Message = "Brand does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateBrandExists(int id)
        {
            bool valid = await _dataValidator.VerifyBrand(id);
            return valid;
        }
    }
}
