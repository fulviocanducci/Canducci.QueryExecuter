namespace Canducci.QueryExecuter.Atrributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public sealed class TableNameAttribute : System.Attribute
    {
        public string Name { get; private set; }
        public TableNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Name = name;            
        }
    }
}
