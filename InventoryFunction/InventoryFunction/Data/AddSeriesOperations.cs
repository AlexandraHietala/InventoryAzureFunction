using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;

namespace InventoryFunction.Data
{
    public interface IAddSeriesOperations
    {
        Task<int> AddSeries(SeriesDto series);
    }

    public class AddSeriesOperations : IAddSeriesOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddSeriesOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<int> AddSeries(SeriesDto series)
        {
            try
            {
                _logger.LogDebug("AddSeries request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //int id = await connection.QueryFirstAsync<int>("[app].[spAddSeries]", new { series_name = series.SERIES_NAME, description = series.SERIES_DESCRIPTION, lastmodifiedby = series.SERIES_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);
                int id = 1;

                _logger.LogInformation("AddSeries success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[400500001] AddSeries Error while inserting series: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[400500002] AddSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[400500003] AddSeries Exception: {e}.");
                throw;
            }
        }
    }
}
