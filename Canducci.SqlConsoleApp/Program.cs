using Canducci.QueryExecuter;
using Canducci.SqlConsoleApp.Models;
using Microsoft.Data.SqlClient;
using SqlKata.Compilers;
using System;

namespace Canducci.SqlConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string cs = "Server=127.0.0.1;Database=Sources;User Id=sa;Password=123456;";
            SourceAsInsert s = new SourceAsInsert(new SqlConnection(cs), new SqlServerCompiler());
            var result0 = s.Insert(new Animal { Description = "Fish" });
            var result1 = s.Insert(new Source { Id = Guid.NewGuid(), Title = "First Source", Created = DateTime.Now.AddDays(-1) });
            Console.WriteLine("Aninal add: {0}", result0.Id);
            Console.WriteLine("Aninal add: {0}", result1.Id);
        }
    }
}
