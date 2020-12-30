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
        private readonly IReadOnlyList<PropertyInfo> _properties;
        private readonly PrimaryKeyAttribute _primaryKeys;
        private readonly TableNameAttribute _tableName;
        private readonly TypeInfo _typeInfoModel;
        private readonly T _model;
        private readonly IReadOnlyDictionary<string, object> _data;

        public ClassDescription(T model)
        {
            _model = model;
            _typeInfoModel = GetTypeInfo();
            _primaryKeys = GetAttribute<PrimaryKeyAttribute>();
            _tableName = GetAttribute<TableNameAttribute>();
            _properties = _typeInfoModel.GetProperties().ToList();
            _data = GetDatas();
        }

        public string GetTableName() => _tableName.Name;
        public IReadOnlyDictionary<string, object> GetData() => _data;
        public bool GetAuto() => _primaryKeys.PrimaryKeyValues.Any(s => s.Auto);
        public IReadOnlyDictionary<string, object> GetPrimaryKeysWithValue()
        {
            Dictionary<string, object> values = new Dictionary<string, object>(_primaryKeys.PrimaryKeyValues.Count);
            _primaryKeys.PrimaryKeyValues.Select(x => x.Name)
                .ToList()
                .ForEach(x =>
                {
                    values.Add(x, _typeInfoModel.GetProperty(x).GetValue(_model));
                });
            return values;            
        }
        public Task SetPrimaryKeyValueInModel<TKey>(TKey value)
        {
            foreach (var primaryKeyValue in _primaryKeys.PrimaryKeyValues)
            {
                PropertyInfo propertyInfo = _properties
                    .Where(c => c.Name == primaryKeyValue.Name)
                    .FirstOrDefault();
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(_model, value);
                }
            }
            return Task.Run(() => { });
        }

        #region Private

        private TAttribute GetAttribute<TAttribute>() where TAttribute: Attribute
        {
            return _typeInfoModel
                .GetCustomAttributes()
                .Where(c => c.GetType() == typeof(TAttribute))
                .OfType<TAttribute>()
                .FirstOrDefault();
        }        

        private TypeInfo GetTypeInfo()
        {
            return _model.GetType().GetTypeInfo();
        }

        private IReadOnlyDictionary<string, object> GetDatas()
        {            
            int countData = _properties.Count - _primaryKeys.PrimaryKeyValues.Where(c => c.Auto).Count();
            IDictionary<string, object> data = new Dictionary<string, object>(countData);
            foreach (PropertyInfo property in _properties)
            {
                foreach (var primaryKeyValue in _primaryKeys.PrimaryKeyValues)
                {
                    if (primaryKeyValue.Auto == true && primaryKeyValue.Name == property.Name)
                    {
                        continue;
                    }
                    data[property.Name] = property.GetValue(_model);
                }
            }
            return (IReadOnlyDictionary<string, object>)data;
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}
