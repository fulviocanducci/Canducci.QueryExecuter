using System.Collections.Generic;
using System.Linq;

namespace Canducci.QueryExecuter.Atrributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public sealed class PrimaryKeyAttribute : System.Attribute
    {
        public IReadOnlyList<PrimaryKeyValue> PrimaryKeyValues { get; private set; }    

        public PrimaryKeyAttribute(string name, bool auto = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }
            PrimaryKeyValues = new List<PrimaryKeyValue> { new PrimaryKeyValue(name, auto) };
        }
        public PrimaryKeyAttribute(params string[] name)
        {
            PrimaryKeyValues = name.Select(x => new PrimaryKeyValue(x)).ToList();
        }
    }
}
