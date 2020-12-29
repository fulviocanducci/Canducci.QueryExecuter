using Canducci.QueryExecuter.Atrributes;

namespace Canducci.SqlConsoleApp.Models
{
    [PrimaryKey("Id")]
    [TableName("Animals")]
    public class Animal
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
