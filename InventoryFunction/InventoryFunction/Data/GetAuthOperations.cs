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
    public interface IGetAuthOperations
    {
        Task<AuthDto> GetAuth(int id);
    }

    public class GetAuthOperations : IGetAuthOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _dataSource;
        private readonly string _userId;
        private readonly string _userPass;
        private readonly string _initialCatalog;
        private readonly SqlConnectionStringBuilder builder;

        public GetAuthOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetAuthOperations>();
            _configuration = configuration;
            _dataSource = _configuration.GetConnectionString("SEInventoryDataSource");
            _userId = _configuration.GetConnectionString("SEInventoryUserId");
            _userPass = _configuration.GetConnectionString("SEInventoryUserPass");
            _initialCatalog = _configuration.GetConnectionString("SEInventoryInitialCatalog");
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = _dataSource;
            builder.UserID = _userId;
            builder.Password = _userPass;
            builder.InitialCatalog = _initialCatalog; ;
        }

        public async Task<AuthDto> GetAuth(int id)
        {
            try
            {
                _logger.LogDebug("GetAuth request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //AuthDto auth = await connection.QueryFirstAsync<AuthDto>("[dbo].[spGetAuth]", new { id }, commandType: CommandType.StoredProcedure);
                AuthDto auth = new AuthDto();

                _logger.LogInformation("GetAuth success response.");
                return auth;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // No records returned
                    _logger.LogInformation($"GetAuth InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500004] Not authenticated.");
                }
                else
                {
                    _logger.LogError($"[100500005] GetAuth InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500006] GetAuth Exception: {e}.");
                throw;
            }
        }
    }
}
