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
    public interface IGetSeriesOperations
    {
        Task<SeriesDto> GetASeries(int id);
        Task<List<SeriesDto>> GetSeries(string search);
    }

    public class GetSeriesOperations : IGetSeriesOperations
    {
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dataSource;
		private readonly string _userId;
		private readonly string _userPass;
		private readonly string _initialCatalog;
		private readonly SqlConnectionStringBuilder builder;

		public GetSeriesOperations(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeriesOperations>();
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

        public async Task<SeriesDto> GetASeries(int id)
        {
            try
            {
                _logger.LogDebug("GetASeries request received.");

                SeriesDto series = new SeriesDto();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    series = connection.Query<SeriesDto>("dbo.spGetASeries", new
                    {
                        id = id
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                _logger.LogInformation("GetASeries success response.");
                return series;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This series doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetASeries InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[400500004] Series does not exist.");
                }
                else
                {
                    _logger.LogError($"[400500005] GetASeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[400500006] GetASeries Exception: {e}.");
                throw;
            }
        }
        public async Task<List<SeriesDto>> GetSeries(string search)
        {
            try
            {
                _logger.LogDebug("GetSeries request received.");

                List<SeriesDto> series = new List<SeriesDto>();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    series = connection.Query<SeriesDto>("dbo.spGetSeries", new
                    {
                        search = search
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                _logger.LogInformation("GetSeries success response.");
                return series.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This series doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetSeries InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[400500007] Seriess do not exist.");
                }
                else
                {
                    _logger.LogError($"[400500008] GetSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[400500009] GetSeries Exception: {e}.");
                throw;
            }
        }
    }
}
