using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Azure.Identity;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System.Collections.Generic;
using System;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IGetItemCommentOperations
    {
        Task<ItemCommentDto> GetItemComment(int id);
        Task<List<ItemCommentDto>> GetItemComments(int itemId);
    }

    public class GetItemCommentOperations : IGetItemCommentOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetItemCommentOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemCommentOperations>();
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

        public async Task<ItemCommentDto> GetItemComment(int id)
        {
            try
            {
                _logger.LogDebug("GetItemComment request received.");

                ItemCommentDto comment = new ItemCommentDto();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    comment = connection.Query<ItemCommentDto>("dbo.spGetItemComment", new
                    {
                        id = id
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("GetItemComment success response.");
                return comment;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This comment doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItemComment InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500007] ItemComment does not exist.");
                }
                else
                {
                    _logger.LogError($"[200500008] GetItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500009] GetItemComment Exception: {e}.");
                throw;
            }
        }
        public async Task<List<ItemCommentDto>> GetItemComments(int itemId)
        {
            try
            {
                _logger.LogDebug("GetItemComments request received.");

                List<ItemCommentDto> comments = new List<ItemCommentDto>();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    comments = connection.Query<ItemCommentDto>("dbo.spGetItemComments", new
                    {
                        item_id = itemId
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                _logger.LogInformation("GetItemComments success response.");
                return comments.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This comment doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItemComments InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500010] ItemComments do not exist.");
                }
                else
                {
                    _logger.LogError($"[200500011] GetItemComments InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500012] GetItemComments Exception: {e}.");
                throw;
            }
        }
    }
}
