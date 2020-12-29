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

            //SourceAsInsert s = new SourceAsInsert(new SqlConnection(cs), new SqlServerCompiler());

            string cs = "Server=127.0.0.1;Database=Sources;User Id=sa;Password=123456;";
            var db = new SqlConnection(cs);
            var compiler = new SqlServerCompiler();
            var r0 = db.Insert(new Animal { Description = "Cat" }, compiler);
            var r1 = db.Insert(new Source { Id = Guid.NewGuid(), Title = "Friends", Created = DateTime.Now.AddDays(-5) }, compiler);
            var r2 = db.Insert(new Caption { AnimalId = Guid.NewGuid(), SourceId = Guid.NewGuid(), Description = "Test Amigo 85" }, compiler);

            
            Console.WriteLine("Aninal add: {0}", r0.Id);
            Console.WriteLine("Aninal add: {0}", r1.Id);
            Console.WriteLine("Aninal add: {0}", r2.Description);
        }
    }
}
