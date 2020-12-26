using Canducci.QueryExecuter.Atrributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Canducci.QueryExecuter.Internals
{
    internal class ClassDescription<T>
    {
        public PropertyInfo[] Properties { get; private set; }
        public PrimaryKeyAttribute PrimaryKey { get; private set; }
        public TableNameAttribute TableName { get; private set; }
        public TypeInfo TypeInfoModel { get; private set; }
        public T Model { get; private set; }
        public IReadOnlyDictionary<string, object> Datas { get; private set; }
        public ClassDescription(T model)
        {
            Model = model;
            TypeInfoModel = GetTypeInfo(model);
            PrimaryKey = GetAttribute<PrimaryKeyAttribute>(TypeInfoModel);
            TableName = GetAttribute<TableNameAttribute>(TypeInfoModel);
            Datas = GetDatas(TypeInfoModel, model, PrimaryKey);
        }

        public Description GetInformation()
        {
            return new Description(Datas, TableName, PrimaryKey);
        }

        public Task SetPrimaryKeyValueInModel<TKey>(TKey value)
        {
            PropertyInfo propertyInfo = Properties
                .Where(c => c.Name == PrimaryKey.Name)
                .FirstOrDefault();
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(Model, value);
            }
            return Task.Run(() => { });
        }

        #region Internal

        internal TAttribute GetAttribute<TAttribute>(TypeInfo typeInfo) where TAttribute : System.Attribute
        {
            return (TAttribute)typeInfo
                .GetCustomAttributes()
                .Where(c => c.GetType() == typeof(TAttribute))
                .FirstOrDefault();
        }
        
        internal TypeInfo GetTypeInfo(T model)
        {
            return model.GetType().GetTypeInfo();
        }

        internal IReadOnlyDictionary<string, object> GetDatas(TypeInfo typeInfo, T model, PrimaryKeyAttribute primaryKey)
        {
            Properties = typeInfo.GetProperties();
            IDictionary<string, object> data = new Dictionary<string, object>(Properties.Length);

            foreach (PropertyInfo property in Properties)
            {
                if (PrimaryKey.Auto == true && PrimaryKey.Name == property.Name)
                {
                    continue;
                }
                data[property.Name] = property.GetValue(model);
            }

            return (IReadOnlyDictionary<string, object>)data;
        }
        #endregion
        
    }
}
