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
    public interface IUpdateUserOperations
    {
        Task UpdateUser(UserDto user);
    }

    public class UpdateUserOperations : IUpdateUserOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public UpdateUserOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateUserOperations>();
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

        public async Task UpdateUser(UserDto user)
        {
            try
            {
                _logger.LogDebug("UpdateUser request received.");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    connection.Query<int>("dbo.spUpdateUser", new
                    {
                        id = user.ID,
                        name = user.NAME,
                        salt = user.PASS_SALT,
                        hash = user.PASS_HASH,
                        role = user.ROLE_ID,
                        lastmodifiedby = user.LAST_MODIFIED_BY
                    },
                        commandType: CommandType.StoredProcedure);
                }

                _logger.LogInformation("UpdateUser success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[100500022] UpdateUser Error while updating user: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[100500023] UpdateUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500024] UpdateUser Exception: {e}.");
                throw;
            }
        }
    }
}
