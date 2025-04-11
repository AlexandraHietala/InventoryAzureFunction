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
        private readonly string _connString;

        public GetCollectionOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<CollectionDto> GetCollection(int id)
        {
            try
            {
                _logger.LogDebug("GetCollection request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //CollectionDto collection = await connection.QueryFirstAsync<CollectionDto>("[app].[spGetCollection]", new { id = id }, commandType: CommandType.StoredProcedure);
                CollectionDto collection = new CollectionDto();

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

                //using IDbConnection connection = new SqlConnection(_connString);
                //IEnumerable<CollectionDto> collections = await connection.QueryAsync<CollectionDto>("[app].[spGetCollections]", new { search = search }, commandType: CommandType.StoredProcedure);
                List<CollectionDto> collections = new List<CollectionDto>();

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
