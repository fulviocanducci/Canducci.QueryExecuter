using Canducci.QueryExecuter.Internals;
using Dapper;
//#if NETSTANDARD2_0
//using Microsoft.Data.SqlClient;
//#else
//using System.Data.SqlClient;
//#endif
using SqlKata;
using SqlKata.Compilers;
using System.Data.Common;
using System.Linq;
//https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-if
namespace Canducci.QueryExecuter
{
    public struct SourceAsInsert
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
            using (ClassDescription<T> classDescription = new ClassDescription<T>(data))
            {
                var auto = classDescription.PrimaryKeys.PrimaryKeyValues.Any(s => s.Auto);
                var query = new Query(classDescription.TableName.Name);
                query.AsInsert(classDescription.Datas, auto);
                var result = Compiler.Compile(query);

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
        }

        //public Task<int> InsertAsync<T>(T data)
        //{
        //    var query = new Query("Animals");
        //    query.AsInsert(data, true);
        //    var result = Compiler.Compile(query);
        //    return Connection.ExecuteScalarAsync<int>(result.Sql, result.NamedBindings);
        //}
    }
}
