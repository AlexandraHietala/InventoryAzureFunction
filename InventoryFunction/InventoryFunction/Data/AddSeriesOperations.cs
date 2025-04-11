using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using InventoryFunction.Models.DTOs;
using System;
using System.Linq;

namespace InventoryFunction.Data
{
    public interface IAddSeriesOperations
    {
        Task<int> AddSeries(SeriesDto series);
    }

    public class AddSeriesOperations : IAddSeriesOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public AddSeriesOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesOperations>();
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

        public async Task<int> AddSeries(SeriesDto series)
        {
            try
            {
                _logger.LogDebug("AddSeries request received.");
                int id = 0;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    id = connection.Query<int>("dbo.spAddSeries", new
                    {
                        series_name = series.SERIES_NAME,
                        description = series.SERIES_DESCRIPTION,
                        lastmodifiedby = series.SERIES_LAST_MODIFIED_BY,
                    },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("AddSeries success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[400500001] AddSeries Error while inserting series: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[400500002] AddSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[400500003] AddSeries Exception: {e}.");
                throw;
            }
        }
    }
}
