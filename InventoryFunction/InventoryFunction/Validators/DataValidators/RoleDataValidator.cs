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

namespace InventoryFunction.Validators.DataValidators
{
    public interface IRoleDataValidator
    {
        Task<bool> VerifyRole(int id);
    }

    public class RoleDataValidator : IRoleDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _dataSource;
        private readonly string _userId;
        private readonly string _userPass;
        private readonly string _initialCatalog;
        private readonly SqlConnectionStringBuilder builder;

        public RoleDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RoleDataValidator>();
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

        public async Task<bool> VerifyRole(int id)
        {
            try
            {
                _logger.LogDebug("VerifyRole request received.");

                RoleDto role = null;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    role = connection.Query<RoleDto>("dbo.spGetRole", new
                    {
                        id = id
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("VerifyRole success response.");
                if (role != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyRole InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyRole Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
