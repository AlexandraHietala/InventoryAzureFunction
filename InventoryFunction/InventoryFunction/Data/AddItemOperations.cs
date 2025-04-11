using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using InventoryFunction.Models.DTOs;
using System.Threading.Tasks;
using System;

namespace InventoryFunction.Data
{
    public interface IAddItemOperations
    {
        Task<int> AddItem(ItemDto item);
    }

    public class AddItemOperations : IAddItemOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddItemOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("StarryEdenItem");
        }

        public async Task<int> AddItem(ItemDto item)
        {
            try
            {
                _logger.LogDebug("AddItem request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //int id = await connection.QueryFirstAsync<int>("[app].[spAddItem]", new { collection_id = item.COLLECTION_ID, status = item.STATUS, type = item.TYPE, brand_id = item.BRAND_ID, series_id = item.SERIES_ID, name = item.NAME, description = item.DESCRIPTION, format = item.FORMAT, size = item.SIZE, year = item.YEAR, photo = item.PHOTO, lastmodifiedby = item.LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);
                int id = 1;

                _logger.LogInformation("AddItem success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500004] AddItem Error while inserting item: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500005] AddItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500006] AddItem Exception: {e}.");
                throw;
            }
        }
    }
}
