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
    public interface IUpdateItemOperations
    {
        Task UpdateItem(ItemDto item);
    }

    public class UpdateItemOperations : IUpdateItemOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public UpdateItemOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemOperations>();
            _configuration = configuration;
            _dataSource = _configuration.GetConnectionString("SEInventoryDataSource");
			_userId = _configuration.GetConnectionString("SEInventoryUserId");
			_userPass = _configuration.GetConnectionString("SEInventoryUserPass");
			_initialCatalog = _configuration.GetConnectionString("SEInventoryInitialCatalog");
			builder = new SqlConnectionStringBuilder();
			builder.DataSource = _dataSource;
			builder.UserID = _userId;
			builder.Password = _userPass;
			builder.InitialCatalog = _initialCatalog;
		}

        public async Task UpdateItem(ItemDto item)
        {
            try
            {
                _logger.LogDebug("UpdateItem request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[dbo].[spUpdateItem]", new { id = item.ID, collection_id = item.COLLECTION_ID, status = item.STATUS, type = item.TYPE, brand_id = item.BRAND_ID, series_id = item.SERIES_ID, name = item.NAME, description = item.DESCRIPTION, format = item.FORMAT, size = item.SIZE, year = item.YEAR, photo = item.PHOTO, lastmodifiedby = item.LAST_MODIFIED_BY  }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateItem success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500031] UpdateItem Error while updating item: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500032] UpdateItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500033] UpdateItem Exception: {e}.");
                throw;
            }
        }
    }
}
