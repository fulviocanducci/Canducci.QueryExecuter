using Canducci.QueryExecuter.Internals;
using Dapper;
//#if NETSTANDARD2_0
//using Microsoft.Data.SqlClient;
//#else
//using System.Data.SqlClient;
//#endif
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Data.Common;
using System.Threading.Tasks;
//https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-if
namespace Canducci.QueryExecuter
{
    public sealed class SourceAsUpdate : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly Compiler _compiler;

        public SourceAsUpdate(DbConnection connection, Compiler compiler)
        {
            _connection = connection;
            _compiler = compiler;
        }

        private SqlResult GetResultUpdate<T>(T data)
        {
            using ClassDescription<T> description = new ClassDescription<T>(data);
            Query query = new Query(description.GetTableName());
            query.AsUpdate(description.GetData());
            foreach (var item in description.GetPrimaryKeysWithValue())
            {
                query.Where(item.Key, item.Value);
            }
            return _compiler.Compile(query);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="data">data</param>
        /// <returns>data</returns>
        public bool Update<T>(T data)
        {
            SqlResult result = GetResultUpdate(data);
            return _connection.Execute(result.Sql, result.NamedBindings) > 0;
        }
        
        /// <summary>
        /// Update Async
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="data">data</param>
        /// <returns>data</returns>
        public async Task<bool> UpdateAsync<T>(T data)
        {
            SqlResult result = GetResultUpdate(data);
            return await (_connection.ExecuteAsync(result.Sql, result.NamedBindings)) > 0;
        }

        #region Dispose
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
