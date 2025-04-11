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
    public interface IUpdateBrandOperations
    {
        Task UpdateBrand(BrandDto brand);
    }

    public class UpdateBrandOperations : IUpdateBrandOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateBrandOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrandOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task UpdateBrand(BrandDto brand)
        {
            try
            {
                _logger.LogDebug("UpdateBrand request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //await connection.QueryFirstAsync<bool>("[app].[spUpdateBrand]", new { id = brand.BRAND_ID, brand_name = brand.BRAND_NAME, description = brand.BRAND_DESCRIPTION, lastmodifiedby = brand.BRAND_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateBrand success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[300500013] UpdateBrand Error while updating brand: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[300500014] UpdateBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500015] UpdateBrand Exception: {e}.");
                throw;
            }
        }
    }
}
