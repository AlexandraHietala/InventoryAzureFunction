using InventoryFunction.Data;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IRemoveSeriesWorkflow
    {
        Task RemoveSeries(int id, string lastmodifiedby);
    }

    public class RemoveSeriesWorkflow : IRemoveSeriesWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveSeriesOperations _removeSeriesOperations;
        private readonly ISeriesDataValidator _dataValidator;
        private readonly ISeriesDeepValidator _deepValidator;

        public RemoveSeriesWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeriesWorkflow>();
            _configuration = configuration;
            _removeSeriesOperations = new RemoveSeriesOperations(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _deepValidator = new SeriesDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task RemoveSeries(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveSeries request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeSeriesOperations.RemoveSeries(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveSeries success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300007] RemoveSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300008] RemoveSeries Exception: {e}.");
                throw;
            }
        }
    }
}