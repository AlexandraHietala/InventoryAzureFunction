using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;

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
        private readonly string _connString;

        public UserDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UserDataValidator>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<bool> VerifyUser(int id)
        {
            try
            {
                _logger.LogDebug("VerifyUser request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //UserDto user = await connection.QueryFirstAsync<UserDto>("[app].[spGetUser]", new { id }, commandType: CommandType.StoredProcedure);
                UserDto user = null;

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
