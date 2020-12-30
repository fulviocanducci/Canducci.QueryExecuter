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

            #region INSERT
            //var animal = new Animal { Description = "Cat" };
            //var r0 = db.Insert(animal, compiler);

            //var source = new Source { Id = Guid.NewGuid(), Title = "Friends", Created = DateTime.Now.AddDays(-5) };
            //var r1 = db.Insert(source, compiler);

            //var caption = new Caption { AnimalId = Guid.NewGuid(), SourceId = Guid.NewGuid(), Description = "Test 1" };
            //var r2 = db.Insert(caption, compiler);


            //Console.WriteLine("Aninal add: {0}", r0.Id);
            //Console.WriteLine("Aninal add: {0}", r1.Id);
            //Console.WriteLine("Aninal add: {0}", r2.Description);
            #endregion

            #region UPDATE
            //var animal = new Animal { Description = "Cat Black", Id = 1 };
            //System.Console.WriteLine(db.Update(animal, compiler));
            var sourceId = Guid.Parse("751EFC32-97E7-4561-A1E6-037700D91CBD");
            var animalId = Guid.Parse("AF6A8CC4-BD20-498D-B518-C7258106A747");
            var caption = new Caption { AnimalId = animalId, SourceId = sourceId, Description = "Test 1 - update" };
            System.Console.WriteLine(db.Update(caption, compiler));
            #endregion
        }
    }
}
