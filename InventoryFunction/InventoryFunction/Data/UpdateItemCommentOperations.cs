using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IUpdateItemCommentOperations
    {
        Task UpdateItemComment(ItemCommentDto comment);
    }

    public class UpdateItemCommentOperations : IUpdateItemCommentOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public UpdateItemCommentOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentOperations>();
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

        public async Task UpdateItemComment(ItemCommentDto comment)
        {
            try
            {
                _logger.LogDebug("UpdateItemComment request received.");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    connection.Execute("dbo.spUpdateItemComment", new
                    {
                        id = comment.COMMENT_ID,
                        item_id = comment.ITEM_ID,
                        comment = comment.COMMENT,
                        lastmodifiedby = comment.COMMENT_LAST_MODIFIED_BY
                    },
                        commandType: CommandType.StoredProcedure);
                }

                _logger.LogInformation("UpdateItemComment success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500028] UpdateItemComment Error while updating comment: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500029] UpdateItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500030] UpdateItemComment Exception: {e}.");
                throw;
            }
        }
    }
}
