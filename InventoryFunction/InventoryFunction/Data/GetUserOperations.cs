using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System.Collections.Generic;
using System;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IGetUserOperations
    {
        Task<UserDto> GetUser(int id);
        Task<List<UserDto>> GetUsers();
    }

    public class GetUserOperations : IGetUserOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetUserOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetUserOperations>();
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

        public async Task<UserDto> GetUser(int id)
        {
            try
            {
                _logger.LogDebug("GetUser request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //UserDto user = await connection.QueryFirstAsync<UserDto>("[dbo].[spGetUser]", new { id }, commandType: CommandType.StoredProcedure);
                UserDto user = new UserDto();

                _logger.LogInformation("GetUser success response.");
                return user;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This user doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetUser InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500013] User does not exist.");
                }
                else
                {
                    _logger.LogError($"[100500014] GetUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500015] GetUser Exception: {e}.");
                throw;
            }
        }
        public async Task<List<UserDto>> GetUsers()
        {
            try
            {
                _logger.LogDebug("GetUsers request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //IEnumerable<UserDto> users = await connection.QueryAsync<UserDto>("[dbo].[spGetUsers]", new { }, commandType: CommandType.StoredProcedure);
                List<UserDto> users = new List<UserDto>();

                _logger.LogInformation("GetUsers success response.");
                return users.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This user doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetUsers InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500016] Users do not exist.");
                }
                else
                {
                    _logger.LogError($"[100500017] GetUsers InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500018] GetUsers Exception: {e}.");
                throw;
            }
        }
    }
}
