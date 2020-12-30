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


        public static bool Update<T>(this DbConnection connection, T data, Compiler compiler)
            where T : class, new()
        {
            return new SourceAsUpdate(connection, compiler).Update(data);
        }

        public static async Task<bool> UpdateAsync<T>(this DbConnection connection, T data, Compiler compiler)
            where T : class, new()
        {
            return await (new SourceAsUpdate(connection, compiler).UpdateAsync(data));
        }

    }
}
