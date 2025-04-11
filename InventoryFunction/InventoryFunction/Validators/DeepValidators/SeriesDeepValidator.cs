using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.System;
using InventoryFunction.Validators.DataValidators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryFunction.Validators.DeepValidators
{
    public interface ISeriesDeepValidator
    {
        Task<string> ValidateAddSeries(Series series);
        Task<string> ValidateUpdateSeries(Series series);
        Task<string> ValidateSeriesId(int id);
    }

    public class SeriesDeepValidator : ISeriesDeepValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesDataValidator _dataValidator;

        public SeriesDeepValidator(ILoggerFactory loggerFactory, IConfiguration configuration, ISeriesDataValidator dataValidator)
        {
            _logger = loggerFactory.CreateLogger<SeriesDeepValidator>();
            _configuration = configuration;
            _dataValidator = dataValidator;
        }

        public async Task<string> ValidateAddSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 400400001, Message = "Series object is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 400400002, Message = "Series object is invalid." });

            if (series != null)
            {
                bool seriesExists = await ValidateSeriesExists(series.Id);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 400400003, Message = "Series does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateSeriesId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool seriesExists = await ValidateSeriesExists(id);
            if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 400400004, Message = "Series does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateSeriesExists(int id)
        {
            bool valid = await _dataValidator.VerifySeries(id);
            return valid;
        }


    }
}
