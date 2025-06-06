﻿using Microsoft.Extensions.Logging;
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
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetRoleOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleOperations>();
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

        public async Task<RoleDto> GetRole(int id)
        {
            try
            {
                _logger.LogDebug("GetRole request received.");

                RoleDto role = new RoleDto();

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

                List<RoleDto> roles = new List<RoleDto>();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    roles = connection.Query<RoleDto>("dbo.spGetRoles", null, commandType: CommandType.StoredProcedure).ToList();
                }

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
