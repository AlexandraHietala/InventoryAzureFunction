﻿using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SqlConnectionStringBuilder = Microsoft.Data.SqlClient.SqlConnectionStringBuilder;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

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
        private readonly string _dataSource;
        private readonly string _userId;
        private readonly string _userPass;
        private readonly string _initialCatalog;
        private readonly SqlConnectionStringBuilder builder;

        public AddBrandOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandOperations>();
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

        public async Task<int> AddBrand(BrandDto brand)
        {
            try
            {
                _logger.LogDebug("AddBrand request received.");
                int id = 0;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    id = connection.Query<int>("dbo.spAddBrand", new
                    {
                        brand_name = brand.BRAND_NAME,
                        description = brand.BRAND_DESCRIPTION,
                        lastmodifiedby = brand.BRAND_LAST_MODIFIED_BY
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

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
