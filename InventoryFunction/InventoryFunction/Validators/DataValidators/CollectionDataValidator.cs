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
    public interface ICollectionDataValidator
    {
        Task<bool> VerifyCollection(int id);
    }

    public class CollectionDataValidator : ICollectionDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _dataSource;
        private readonly string _userId;
        private readonly string _userPass;
        private readonly string _initialCatalog;
        private readonly SqlConnectionStringBuilder builder;

        public CollectionDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<CollectionDataValidator>();
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

        public async Task<bool> VerifyCollection(int id)
        {
            try
            {
                _logger.LogDebug("VerifyCollection request received.");

                CollectionDto collection = null;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    collection = connection.Query<CollectionDto>("dbo.spGetCollection", new
                    {
                        id = id
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("VerifyCollection success response.");
                if (collection != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyCollection InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyCollection Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
