using Canducci.QueryExecuter.Atrributes;
using System.Collections.Generic;

namespace Canducci.QueryExecuter.Internals
{
    public struct Description
    {
        public IReadOnlyDictionary<string, object> Datas { get; }
        public TableNameAttribute TableName { get; }
        public PrimaryKeyAttribute PrimaryKey { get; }
        public Description(
            IReadOnlyDictionary<string, object> datas,
            TableNameAttribute tableName,
            PrimaryKeyAttribute primaryKey
        )
        {
            Datas = datas;
            TableName = tableName;
            PrimaryKey = primaryKey;
        }
    }
}
