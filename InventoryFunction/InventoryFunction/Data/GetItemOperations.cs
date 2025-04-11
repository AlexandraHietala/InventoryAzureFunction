using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Azure.Identity;
using InventoryFunction.Models.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IGetItemOperations
    {
        Task<ItemDto> GetItem(int id);
        Task<List<ItemDto>> GetItems(string search);
        Task<List<ItemDto>> GetItemsPerCollection(int collectionId);
    }

    public class GetItemOperations : IGetItemOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetItemOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemOperations>();
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

        public async Task<ItemDto> GetItem(int id)
        {
            try
            {
                _logger.LogDebug("GetItem request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //ItemDto item = await connection.QueryFirstAsync<ItemDto>("[dbo].[spGetItem]", new { id = id }, commandType: CommandType.StoredProcedure);
                ItemDto item = new ItemDto();

                _logger.LogInformation("GetItem success response.");
                return item;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This item doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItem InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500013] Item does not exist.");
                }
                else
                {
                    _logger.LogError($"[200500014] GetItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500015] GetItem Exception: {e}.");
                throw;
            }
        }
        public async Task<List<ItemDto>> GetItems(string search)
        {
            try
            {
                _logger.LogDebug("GetItems request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //IEnumerable<ItemDto> items = await connection.QueryAsync<ItemDto>("[dbo].[spGetItems]", new { search = search }, commandType: CommandType.StoredProcedure);
                List<ItemDto> items = new List<ItemDto>();

                _logger.LogInformation("GetItems success response.");
                return items.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This item doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItems InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500016] Items do not exist.");
                }
                else
                {
                    _logger.LogError($"[200500017] GetItems InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500018] GetItems Exception: {e}.");
                throw;
            }
        }

        public async Task<List<ItemDto>> GetItemsPerCollection(int collectionId)
        {
            try
            {
                _logger.LogDebug("GetItemsPerCollection request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //IEnumerable<ItemDto> items = await connection.QueryAsync<ItemDto>("[dbo].[spGetItemsPerCollection]", new { collection_id = collectionId }, commandType: CommandType.StoredProcedure);
                List<ItemDto> items = new List<ItemDto>();

                _logger.LogInformation("GetItemsPerCollection success response.");
                return items.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This item doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItemsPerCollection InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500019] Items do not exist.");
                }
                else
                {
                    _logger.LogError($"[200500020] GetItemsPerCollection InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500021] GetItemsPerCollection Exception: {e}.");
                throw;
            }
        }
    }
}
