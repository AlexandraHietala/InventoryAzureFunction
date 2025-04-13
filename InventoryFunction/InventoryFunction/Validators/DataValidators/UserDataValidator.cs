using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using System.Linq;

namespace InventoryFunction.Validators.DataValidators
{
    public interface IUserDataValidator
    {
        Task<bool> VerifyUser(int id);
    }

    public class UserDataValidator : IUserDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _dataSource;
        private readonly string _userId;
        private readonly string _userPass;
        private readonly string _initialCatalog;
        private readonly SqlConnectionStringBuilder builder;

        public UserDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UserDataValidator>();
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

        public async Task<bool> VerifyUser(int id)
        {
            try
            {
                _logger.LogDebug("VerifyUser request received.");

                UserDto user = null;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    user = connection.Query<UserDto>("dbo.spGetUser", new
                    {
                        id = id
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("VerifyUser success response.");
                if (user != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyUser InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyUser Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
