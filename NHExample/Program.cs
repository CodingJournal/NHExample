using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHExample
{
    class Program
    {
        static void Main(string[] args)
        {

            // Initialize NHibernate
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.Product).Assembly);

            // Get ourselves an NHibernate Session
            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();

            // Create the database schema
            new SchemaExport(cfg).Create(true, true);

            // Create a Product...
            var product = new Domain.Product
                        {
                            Name = "Some C# Book",
                            Price = 500,
                            Category = "Books"
                        };

            // And save it to the database
            sess.Save(product);
            sess.Flush();

            // Note that we do not use the table name specified
            // in the mapping, but the class name, which is a nice
            // abstraction that comes with NHibernate
            IQuery q = sess.CreateQuery("FROM Product");
            var list = q.List<Domain.Product>();

            // List all the entries' names
            list.ToList().ForEach( p => Console.WriteLine( p.Name ));


            // Don't close the application right away, so we can read
            // the output.
            Console.ReadLine();
        }
    }
}
