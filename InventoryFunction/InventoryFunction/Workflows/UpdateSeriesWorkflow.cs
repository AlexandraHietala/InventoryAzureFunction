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
    public interface IUpdateSeriesWorkflow
    {
        Task UpdateSeries(Series series);
    }

    public class UpdateSeriesWorkflow : IUpdateSeriesWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateSeriesOperations _updateSeriesOperations;
        private readonly ISeriesDataValidator _dataValidator;
        private readonly ISeriesDeepValidator _deepValidator;

        public UpdateSeriesWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesWorkflow>();
            _configuration = configuration;
            _updateSeriesOperations = new UpdateSeriesOperations(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _deepValidator = new SeriesDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task UpdateSeries(Series series)
        {
            _logger.LogDebug("UpdateSeries request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUpdateSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                SeriesDto seriesDto = SeriesConverter.ConvertSeriesToSeriesDto(series);
                await _updateSeriesOperations.UpdateSeries(seriesDto);

                // Respond
                _logger.LogInformation("UpdateSeries success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300009] UpdateSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300010] UpdateSeries Exception: {e}.");
                throw;
            }
        }
    }
}