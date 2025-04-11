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
    public interface IAddSeriesWorkflow
    {
        Task<int> AddSeries(Series series);
    }

    public class AddSeriesWorkflow : IAddSeriesWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddSeriesOperations _addSeriesOperations;
        private readonly ISeriesDataValidator _dataValidator;
        private readonly ISeriesDeepValidator _deepValidator;


        public AddSeriesWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesWorkflow>();
            _configuration = configuration;
            _addSeriesOperations = new AddSeriesOperations(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _deepValidator = new SeriesDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task<int> AddSeries(Series series)
        {
            _logger.LogDebug("AddSeries request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateAddSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                SeriesDto seriesDto = SeriesConverter.ConvertSeriesToSeriesDto(series);
                int newSeriesId = await _addSeriesOperations.AddSeries(seriesDto);

                // Respond
                _logger.LogInformation("AddSeries success response.");
                return newSeriesId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300001] AddSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300002] AddSeries Exception: {e}.");
                throw;
            }
        }
    }
}