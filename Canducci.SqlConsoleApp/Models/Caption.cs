using Canducci.QueryExecuter.Atrributes;

namespace Canducci.SqlConsoleApp.Models
{
    [PrimaryKey("SourceId", "AnimalId")]
    [TableName("Captions")]
    public class Caption
    {
        public System.Guid SourceId { get; set; }
        public System.Guid AnimalId { get; set; }
        public string Description { get; set; }
    }
}
