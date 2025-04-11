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
    public interface IRoleOperations
    {
        Task<RoleDto> GetRole(int id);
        Task<List<RoleDto>> GetRoles();
    }

    public class GetRoleOperations : IRoleOperations
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetRoleOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleOperations>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<RoleDto> GetRole(int id)
        {
            try
            {
                _logger.LogDebug("GetRole request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //RoleDto role = await connection.QueryFirstAsync<RoleDto>("[app].[spGetRole]", new { id }, commandType: CommandType.StoredProcedure);
                RoleDto role = new RoleDto();

                _logger.LogInformation("GetRole success response.");
                return role;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This role doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetRole InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500007] Role does not exist.");
                }
                else
                {
                    _logger.LogError($"[100500008] GetRole InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500009] GetRole Exception: {e}.");
                throw;
            }
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            try
            {
                _logger.LogDebug("GetRoles request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //IEnumerable<RoleDto> roles = await connection.QueryAsync<RoleDto>("[app].[spGetRoles]", new { }, commandType: CommandType.StoredProcedure);
                List<RoleDto> roles = new List<RoleDto>();

                _logger.LogInformation("GetRoles success response.");
                return roles.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This role doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetRoles InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500010] Roles do not exist.");
                }
                else
                {
                    _logger.LogError($"[100500011] GetRoles InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500012] GetRoles Exception: {e}.");
                throw;
            }
        }
    }
}
