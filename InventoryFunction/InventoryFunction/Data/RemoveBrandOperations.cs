using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System;

namespace InventoryFunction.Data
{
    public interface IRemoveBrandOperations
    {
        Task RemoveBrand(int id, string lastmodifiedby);
    }

    public class RemoveBrandOperations : IRemoveBrandOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveBrandOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("StarryEdenBrand")!;
        }

        public async Task RemoveBrand(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveBrand request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[app].[spRemoveBrand]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveBrand success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[300500010] Error while removing brand: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[300500011] RemoveBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500012] RemoveBrand Exception: {e}.");
                throw;
            }
        }

    }
}
