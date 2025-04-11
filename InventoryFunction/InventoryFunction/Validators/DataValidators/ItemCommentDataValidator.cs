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
    public interface IItemCommentDataValidator
    {
        Task<bool> VerifyItemComment(int id);
    }

    public class ItemCommentDataValidator : IItemCommentDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public ItemCommentDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ItemCommentDataValidator>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory");
        }

        public async Task<bool> VerifyItemComment(int id)
        {
            try
            {
                _logger.LogDebug("VerifyItemComment request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //ItemCommentDto comment = await connection.QueryFirstAsync<ItemCommentDto>("[dbo].[spGetItemComment]", new { id = id }, commandType: CommandType.StoredProcedure);
                ItemCommentDto comment = new ItemCommentDto();

                _logger.LogInformation("VerifyItemComment success response.");
                if (comment != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyItemComment InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyItemComment Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
