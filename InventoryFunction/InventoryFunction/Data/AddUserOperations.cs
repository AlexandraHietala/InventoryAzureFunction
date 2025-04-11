using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using InventoryFunction.Models.Classes;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IAddUserOperations
    {
        Task<int> AddUser(UserDto user);
    }

    public class AddUserOperations : IAddUserOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public AddUserOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUserOperations>();
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

        public async Task<int> AddUser(UserDto user)
        {
            try
            {
                _logger.LogDebug("AddUser request received.");
                int id = 0;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    id = connection.Query<int>("dbo.spAddUser", new
                    {
                        name = user.NAME,
                        salt = user.PASS_SALT,
                        hash = user.PASS_HASH,
                        role = user.ROLE_ID,
                        lastmodifiedby = user.LAST_MODIFIED_BY
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("AddUser success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[100500001] AddUser Error while inserting user: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[100500002] AddUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500003] AddUser Exception: {e}.");
                throw;
            }
        }
    }
}
