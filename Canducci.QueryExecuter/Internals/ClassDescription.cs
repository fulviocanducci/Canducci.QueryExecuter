using Canducci.QueryExecuter.Atrributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Canducci.QueryExecuter.Internals
{
    internal class ClassDescription<T> : IDisposable
    {
        public IReadOnlyList<PropertyInfo> Properties { get; private set; }
        public PrimaryKeyAttribute PrimaryKeys { get; private set; }
        public TableNameAttribute TableName { get; private set; }
        public TypeInfo TypeInfoModel { get; private set; }
        public T Model { get; private set; }
        public IReadOnlyDictionary<string, object> Datas { get; private set; }

        public ClassDescription(T model)
        {
            Model = model;
            TypeInfoModel = GetTypeInfo();
            PrimaryKeys = GetAttribute<PrimaryKeyAttribute>();
            TableName = GetAttribute<TableNameAttribute>();
            Datas = GetDatas();
        }

        public Task SetPrimaryKeyValueInModel<TKey>(TKey value)
        {
            foreach (var primaryKeyValue in PrimaryKeys.PrimaryKeyValues)
            {
                PropertyInfo propertyInfo = Properties
                    .Where(c => c.Name == primaryKeyValue.Name)
                    .FirstOrDefault();
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(Model, value);
                }
            }
            return Task.Run(() => { });
        }

        #region Internal

        internal TAttribute GetAttribute<TAttribute>() where TAttribute: Attribute
        {
            return TypeInfoModel
                .GetCustomAttributes()
                .Where(c => c.GetType() == typeof(TAttribute))
                .OfType<TAttribute>()
                .FirstOrDefault();
        }        

        internal TypeInfo GetTypeInfo()
        {
            return Model.GetType().GetTypeInfo();
        }

        internal IReadOnlyDictionary<string, object> GetDatas()
        {
            Properties = TypeInfoModel.GetProperties().ToList();
            int countData = Properties.Count - PrimaryKeys.PrimaryKeyValues.Where(c => c.Auto).Count();
            IDictionary<string, object> data = new Dictionary<string, object>(countData);
            foreach (PropertyInfo property in Properties)
            {
                foreach (var primaryKeyValue in PrimaryKeys.PrimaryKeyValues)
                {
                    if (primaryKeyValue.Auto == true && primaryKeyValue.Name == property.Name)
                    {
                        continue;
                    }
                    data[property.Name] = property.GetValue(Model);
                }
            }
            return (IReadOnlyDictionary<string, object>)data;
        }

        #endregion

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}
