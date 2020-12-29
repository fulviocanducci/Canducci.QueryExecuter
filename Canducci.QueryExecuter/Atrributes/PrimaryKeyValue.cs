namespace Canducci.QueryExecuter.Atrributes
{
    public sealed class PrimaryKeyValue
    {
        public PrimaryKeyValue(string name, bool auto = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Name = name;
            Auto = auto;
        }
        public string Name { get; private set; }
        public bool Auto { get; private set; }
    }
}
