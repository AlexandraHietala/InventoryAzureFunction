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
    public interface ISeriesDataValidator
    {
        Task<bool> VerifySeries(int id);
    }

    public class SeriesDataValidator : ISeriesDataValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public SeriesDataValidator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<SeriesDataValidator>();
            _configuration = configuration;
            //_connString = _configuration.GetConnectionString("SEInventory");
        }

        public async Task<bool> VerifySeries(int id)
        {
            try
            {
                _logger.LogDebug("VerifySeries request received.");

                //using IDbConnection connection = new SqlConnection(_connString);
                //SeriesDto series = await connection.QueryFirstAsync<SeriesDto>("[app].[spGetASeries]", new { id }, commandType: CommandType.StoredProcedure);
                SeriesDto series = new SeriesDto();

                _logger.LogInformation("VerifySeries success response.");
                if (series != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifySeries InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifySeries Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
