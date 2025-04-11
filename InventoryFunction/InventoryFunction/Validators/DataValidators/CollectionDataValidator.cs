using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;

namespace InventoryFunction.Validators.DataValidators
{
    public interface ICollectionDataValidator
    {
        Task<bool> VerifyCollection(int id);
    }

    public class CollectionDataValidator : ICollectionDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public CollectionDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<CollectionDataValidator>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory");
        }

        public async Task<bool> VerifyCollection(int id)
        {
            try
            {
                _logger.LogDebug("VerifyCollection request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //CollectionDto collection = await connection.QueryFirstAsync<CollectionDto>("[app].[spGetCollection]", new { id }, commandType: CommandType.StoredProcedure);
                CollectionDto collection = new CollectionDto();

                _logger.LogInformation("VerifyCollection success response.");
                if (collection != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyCollection InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyCollection Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
