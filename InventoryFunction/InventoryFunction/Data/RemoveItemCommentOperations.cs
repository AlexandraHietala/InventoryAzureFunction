﻿using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System;

namespace InventoryFunction.Data
{
    public interface IRemoveItemCommentOperations
    {
        Task RemoveItemComment(int id, string lastmodifiedby);
    }

    public class RemoveItemCommentOperations : IRemoveItemCommentOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveItemCommentOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemCommentOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task RemoveItemComment(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveItemComment request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[app].[spRemoveItemComment]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveItemComment success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500022] Error while removing comment: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500023] RemoveItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500024] RemoveItemComment Exception: {e}.");
                throw;
            }
        }

    }
}
