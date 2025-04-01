using Dapper;
using Microsoft.Data.SqlClient;

namespace LeadwaycanteenApi.Wrappers
{
    /// <summary>
    /// DbWrapper for database managing database trips
    /// </summary>
    /// <typeparam name="T">variable type</typeparam>
    /// <param name="config">the configuration type</param>
    public class DbWrapper<T>(IConfiguration config) where T : class
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(config), "Connection string 'DefaultConnection' not found.");

        private async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        /// Executes the sql query
        /// </summary>
        /// <param name="sql">sql statement</param>
        /// <param name="param">the parameters</param>
        /// <returns>true or false</returns>
        public async Task<bool> ExecuteAsync(string sql, object? param = null)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                var result = await connection.ExecuteAsync(sql, param);
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Querys the database for information
        /// </summary>
        /// <param name="sql">the saql query statement</param>
        /// <param name="param">the list of parameters</param>
        /// <returns>a list of data</returns>
        public async Task<IEnumerable<T>> QueryAsync(string sql, object? param = null)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                return await connection.QueryAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        /// <summary>
        /// Gets a single data from the database
        /// </summary>
        /// <param name="sql">the sql statement to get the query</param>
        /// <param name="param">the parameter list</param>
        /// <returns>an item from the database</returns>
        public async Task<T?> QuerySingleOrDefaultAsync(string sql, object? param = null)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// gets the first item from the database
        /// </summary>
        /// <param name="sql">the sql statement for the query</param>
        /// <param name="param">a list of parameters</param>
        /// <returns>a single item from the database</returns>
        public async Task<T?> QueryFirstOrDefaultAsync(string sql, object? param = null)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Seriously 😒😒
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> ExecuteScalarAsync(string sql, object? param = null)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                return await connection.ExecuteScalarAsync<int>(sql, param);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
