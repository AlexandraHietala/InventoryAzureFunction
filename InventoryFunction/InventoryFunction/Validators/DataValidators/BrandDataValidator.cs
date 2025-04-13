using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using InventoryFunction.Data;
using System.Linq;

namespace InventoryFunction.Validators.DataValidators
{
    public interface IBrandDataValidator
    {
        Task<bool> VerifyBrand(int id);
    }

    public class BrandDataValidator : IBrandDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _dataSource;
        private readonly string _userId;
        private readonly string _userPass;
        private readonly string _initialCatalog;
        private readonly SqlConnectionStringBuilder builder;

        public BrandDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<BrandDataValidator>();
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

        public async Task<bool> VerifyBrand(int id)
        {
            try
            {
                _logger.LogDebug("VerifyBrand request received.");

                BrandDto brand = null;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    brand = connection.Query<BrandDto>("dbo.spGetBrand", new
                    {
                        id = id
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("VerifyBrand success response.");
                if (brand != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyBrand InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyBrand Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }

    }
}
