﻿using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;

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
        private readonly string _connString;

        public BrandDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<BrandDataValidator>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory");
        }

        public async Task<bool> VerifyBrand(int id)
        {
            try
            {
                _logger.LogDebug("VerifyBrand request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //BrandDto brand = await connection.QueryFirstAsync<BrandDto>("[app].[spGetBrand]", new { id }, commandType: CommandType.StoredProcedure);
                BrandDto brand = new BrandDto();

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
