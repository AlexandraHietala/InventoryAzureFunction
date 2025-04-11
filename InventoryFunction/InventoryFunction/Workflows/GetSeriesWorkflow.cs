﻿using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IGetSeriesWorkflow
    {
        Task<Series> GetASeries(int id);
        Task<List<Series>> GetSeries();
    }

    public class GetSeriesWorkflow : IGetSeriesWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetSeriesOperations _getSeriesOperations;
        private readonly ISeriesDataValidator _dataValidator;
        private readonly ISeriesDeepValidator _deepValidator;

        public GetSeriesWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeriesWorkflow>();
            _configuration = configuration;
            _getSeriesOperations = new GetSeriesOperations(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _deepValidator = new SeriesDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task<Series> GetASeries(int id)
        {
            _logger.LogDebug("GetASeries request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                SeriesDto seriesDto = await _getSeriesOperations.GetASeries(id);
                Series series = SeriesConverter.ConvertSeriesDtoToSeries(seriesDto);

                // Respond
                _logger.LogInformation("GetASeries success response.");
                return series;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300003] GetASeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300004] GetSeries Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Series>> GetSeries()
        {
            _logger.LogDebug("GetSeries request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<SeriesDto> seriesDtos = await _getSeriesOperations.GetSeries(null);
                List<Series> series = SeriesConverter.ConvertListSeriesDtoToListSeries(seriesDtos);

                // Respond
                _logger.LogInformation("GetASeries success response.");
                return series;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300005] GetASeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300006] GetASeries Exception: {e}.");
                throw;
            }
        }
    }
}