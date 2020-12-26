using Canducci.QueryExecuter.Atrributes;
using System;

namespace Canducci.SqlConsoleApp.Models
{
    [PrimaryKey("Id")]
    [TableName("Animals")]
    public class Animal
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    [PrimaryKey("Id", false)]
    [TableName("Sources")]
    public class Source
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
    }
}
