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
    public interface IUpdateSeriesOperations
    {
        Task UpdateSeries(SeriesDto series);
    }

    public class UpdateSeriesOperations : IUpdateSeriesOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateSeriesOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("StarryEdenSeries")!;
        }

        public async Task UpdateSeries(SeriesDto series)
        {
            try
            {
                _logger.LogDebug("UpdateSeries request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[app].[spUpdateSeries]", new { id = series.SERIES_ID, series_name = series.SERIES_NAME, description = series.SERIES_DESCRIPTION, lastmodifiedby = series.SERIES_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateSeries success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[400500013] UpdateSeries Error while updating series: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[400500014] UpdateSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[400500015] UpdateSeries Exception: {e}.");
                throw;
            }
        }
    }
}
