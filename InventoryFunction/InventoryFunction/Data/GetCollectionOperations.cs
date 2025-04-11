using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Azure.Identity;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IGetCollectionOperations
    {
        Task<CollectionDto> GetCollection(int id);
        Task<List<CollectionDto>> GetCollections(string search);
    }

    public class GetCollectionOperations : IGetCollectionOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetCollectionOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionOperations>();
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

        public async Task<CollectionDto> GetCollection(int id)
        {
            try
            {
                _logger.LogDebug("GetCollection request received.");
                CollectionDto collection = new CollectionDto();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    collection = connection.Query<CollectionDto>("dbo.spGetCollection", new
                    {
                        id = id
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault(); //TODO: finish converting to this format
                }

                _logger.LogInformation("GetCollection success response.");
                return collection;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This collection doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetCollection InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[500500004] Collection does not exist.");
                }
                else
                {
                    _logger.LogError($"[500500005] GetCollection InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[500500006] GetCollection Exception: {e}.");
                throw;
            }
        }
        public async Task<List<CollectionDto>> GetCollections(string search)
        {
            try
            {
                _logger.LogDebug("GetCollections request received.");

                List<CollectionDto> collections = new List<CollectionDto>();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    collections = connection.Query<CollectionDto>("dbo.spGetCollections", new
                    {
                        search = search
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                _logger.LogInformation("GetCollections success response.");
                return collections.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This collection doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetCollections InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[500500007] Collections do not exist.");
                }
                else
                {
                    _logger.LogError($"[500500008] GetCollections InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[500500009] GetCollections Exception: {e}.");
                throw;
            }
        }
    }
}
