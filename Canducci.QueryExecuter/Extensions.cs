using SqlKata.Compilers;
using System.Data.Common;
using System.Threading.Tasks;

namespace Canducci.QueryExecuter
{
    public static class Extensions
    {
        public static T Insert<T>(this DbConnection connection, T data, Compiler compiler)
            where T : class, new()
        {
            return new SourceAsInsert(connection, compiler).Insert(data);
        }

        public static async Task<T> InsertAsync<T>(this DbConnection connection, T data, Compiler compiler)
            where T : class, new()
        {
            return await (new SourceAsInsert(connection, compiler).InsertAsync(data));
        }
    }
}
