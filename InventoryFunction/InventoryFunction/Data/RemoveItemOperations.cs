﻿using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System;

namespace InventoryFunction.Data
{
    public interface IRemoveItemOperations
    {
        Task RemoveItem(int id, string lastmodifiedby);
    }

    public class RemoveItemOperations : IRemoveItemOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveItemOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task RemoveItem(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveItem request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[app].[spRemoveItem]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveItem success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500025] Error while removing item: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500026] RemoveItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500027] RemoveItem Exception: {e}.");
                throw;
            }
        }

    }
}
