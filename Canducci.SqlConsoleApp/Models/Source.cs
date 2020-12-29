using Canducci.QueryExecuter.Atrributes;
using System;

namespace Canducci.SqlConsoleApp.Models
{
    [PrimaryKey("Id", false)]
    [TableName("Sources")]
    public class Source
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
    }
}
