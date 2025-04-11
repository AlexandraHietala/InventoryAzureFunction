using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Azure.Identity;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IGetBrandOperations
    {
        Task<BrandDto> GetBrand(int id);
        Task<List<BrandDto>> GetBrands(string search);
    }

    public class GetBrandOperations : IGetBrandOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetBrandOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrandOperations>();
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

        public async Task<BrandDto> GetBrand(int id)
        {
            try
            {
                _logger.LogDebug("GetBrand request received.");

				BrandDto brand = new BrandDto();

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

                _logger.LogInformation("GetBrand success response.");
                return brand;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This brand doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetBrand InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500004] Brand does not exist.");
                }
                else
                {
                    _logger.LogError($"[300500005] GetBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500006] GetBrand Exception: {e}.");
                throw;
            }
        }
        public async Task<List<BrandDto>> GetBrands(string search)
        {
            try
            {
                _logger.LogDebug("GetBrands request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //IEnumerable<BrandDto> brands = await connection.QueryAsync<BrandDto>("[dbo].[spGetBrands]", new { search = search }, commandType: CommandType.StoredProcedure);
                List<BrandDto> brands = new List<BrandDto>();

                _logger.LogInformation("GetBrands success response.");
                return brands.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This brand doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetBrands InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500007] Brands do not exist.");
                }
                else
                {
                    _logger.LogError($"[300500008] GetBrands InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500009] GetBrands Exception: {e}.");
                throw;
            }
        }
    }
}
