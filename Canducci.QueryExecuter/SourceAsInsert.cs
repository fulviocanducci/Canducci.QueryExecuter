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
using System.Linq;
using System.Threading.Tasks;
//https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-if
namespace Canducci.QueryExecuter
{
    public sealed class SourceAsInsert: IDisposable
    {

        private readonly DbConnection Connection;
        private readonly Compiler Compiler;

        public SourceAsInsert(DbConnection connection, Compiler compiler)
        {
            Connection = connection;
            Compiler = compiler;
        }


        public T Insert<T>(T data)
        {
            using ClassDescription<T> classDescription = new ClassDescription<T>(data);
            bool auto = classDescription.PrimaryKeys.PrimaryKeyValues.Any(s => s.Auto);
            Query query = new Query(classDescription.TableName.Name);
            query.AsInsert(classDescription.Datas, auto);
            SqlResult result = Compiler.Compile(query);
            if (auto)
            {
                int value = Connection.ExecuteScalar<int>(result.Sql, result.NamedBindings);
                classDescription.SetPrimaryKeyValueInModel(value);
            }
            else
            {
                Connection.Execute(result.Sql, result.NamedBindings);
            }

            return data;
        }

        public async Task<T> InsertAsync<T>(T data)
        {
            using ClassDescription<T> classDescription = new ClassDescription<T>(data);
            bool auto = classDescription.PrimaryKeys.PrimaryKeyValues.Any(s => s.Auto);
            Query query = new Query(classDescription.TableName.Name);
            query.AsInsert(classDescription.Datas, auto);
            SqlResult result = Compiler.Compile(query);
            if (auto)
            {
                int value = await Connection.ExecuteScalarAsync<int>(result.Sql, result.NamedBindings);
                await classDescription.SetPrimaryKeyValueInModel(value);
            }
            else
            {
                await Connection.ExecuteAsync(result.Sql, result.NamedBindings);
            }

            return data;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
