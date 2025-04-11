using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;

namespace InventoryFunction.Data
{
    public interface IAddBrandOperations
    {
        Task<int> AddBrand(BrandDto brand);
    }

    public class AddBrandOperations : IAddBrandOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddBrandOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<int> AddBrand(BrandDto brand)
        {
            try
            {
                _logger.LogDebug("AddBrand request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //int id = await connection.QueryFirstAsync<int>("[app].[spAddBrand]", new { brand_name = brand.BRAND_NAME, description = brand.BRAND_DESCRIPTION, lastmodifiedby = brand.BRAND_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);
                int id = 1;

                _logger.LogInformation("AddBrand success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[300500001] AddBrand Error while inserting brand: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[300500002] AddBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500003] AddBrand Exception: {e}.");
                throw;
            }
        }
    }
}
