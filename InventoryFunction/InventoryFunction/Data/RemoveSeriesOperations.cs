using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System;

namespace InventoryFunction.Data
{
    public interface IRemoveSeriesOperations
    {
        Task RemoveSeries(int id, string lastmodifiedby);
    }

    public class RemoveSeriesOperations : IRemoveSeriesOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveSeriesOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeriesOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task RemoveSeries(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveSeries request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[app].[spRemoveSeries]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveSeries success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[400500010] Error while removing series: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[400500011] RemoveSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[4005000 12] RemoveSeries Exception: {e}.");
                throw;
            }
        }

    }
}
