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
    public interface IAddItemCommentOperations
    {
        Task<int> AddItemComment(ItemCommentDto comment);
    }

    public class AddItemCommentOperations : IAddItemCommentOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddItemCommentOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemCommentOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory");
        }

        public async Task<int> AddItemComment(ItemCommentDto comment)
        {
            try
            {
                _logger.LogDebug("AddItemComment request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //int id = await connection.QueryFirstAsync<int>("[app].[spAddItemComment]", new { item_id = comment.ITEM_ID, comment = comment.COMMENT, lastmodifiedby = comment.COMMENT_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);
                int id = 1;

                _logger.LogInformation("AddItemComment success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500001] AddItemComment Error while inserting comment: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500002] AddItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500003] AddItemComment Exception: {e}.");
                throw;
            }
        }
    }
}
