namespace Canducci.QueryExecuter.Atrributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public sealed class PrimaryKeyAttribute : System.Attribute
    {
        public string Name { get; private set; }
        public bool Auto { get; private set; }
        public PrimaryKeyAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Name = name;
            Auto = true;
        }
        public PrimaryKeyAttribute(string name, bool auto)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Name = name;
            Auto = auto;
        }

    }
}
