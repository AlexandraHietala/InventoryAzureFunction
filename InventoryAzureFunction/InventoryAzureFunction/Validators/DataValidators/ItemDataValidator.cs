using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using SEInventoryCollection.Models.DTOs;
using System;

namespace SEInventoryCollection.Validators.DataValidators
{
    public interface IItemDataValidator
    {
        Task<bool> VerifyItem(int id);
    }

    public class ItemDataValidator : IItemDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public ItemDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ItemDataValidator>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("StarryEdenItem");
        }

        public async Task<bool> VerifyItem(int id)
        {
            try
            {
                _logger.LogDebug("VerifyItem request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //ItemDto item = await connection.QueryFirstAsync<ItemDto>("[app].[spGetItem]", new { id }, commandType: CommandType.StoredProcedure);
                ItemDto item = new ItemDto();

                _logger.LogInformation("VerifyItem success response.");
                if (item != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyItem InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyItem Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
