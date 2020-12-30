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
    public sealed class SourceAsInsert : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly Compiler _compiler;

        public SourceAsInsert(DbConnection connection, Compiler compiler)
        {
            _connection = connection;
            _compiler = compiler;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="data">data</param>
        /// <returns>data</returns>

        public T Insert<T>(T data)
        {
            using ClassDescription<T> description = new ClassDescription<T>(data);
            bool auto = description.GetAuto();
            Query query = new Query(description.GetTableName());
            query.AsInsert(description.GetData(), auto);
            SqlResult result = _compiler.Compile(query);
            if (auto)
            {
                int value = _connection.ExecuteScalar<int>(result.Sql, result.NamedBindings);
                description.SetPrimaryKeyValueInModel(value);
            }
            else
            {
                _connection.Execute(result.Sql, result.NamedBindings);
            }

            return data;
        }

        /// <summary>
        /// Insert Async
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="data">data</param>
        /// <returns>data</returns>
        public async Task<T> InsertAsync<T>(T data)
        {
            using ClassDescription<T> description = new ClassDescription<T>(data);
            bool auto = description.GetAuto();
            Query query = new Query(description.GetTableName());
            query.AsInsert(description.GetData(), auto);
            SqlResult result = _compiler.Compile(query);
            if (auto)
            {
                int value = await _connection.ExecuteScalarAsync<int>(result.Sql, result.NamedBindings);
                await description.SetPrimaryKeyValueInModel(value);
            }
            else
            {
                await _connection.ExecuteAsync(result.Sql, result.NamedBindings);
            }

            return data;
        }

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
