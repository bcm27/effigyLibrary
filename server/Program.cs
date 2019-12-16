using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var myData = new Data();
            var mySchema = myData.createSchemas();

            var root = new { Hello = "Hello World" };
            var json = mySchema.Execute(_ =>
            {
                _.Query = " { hello}";
                _.Root = root;
            });

            Console.WriteLine(json);
        }
    } // end class Program

    class Data
    {
        public Data() { }
        public ISchema createSchemas()
        {

            ISchema myRet = Schema.For(@"
            type Account {
                name: String,
                side: String
            }
            type Query {
                    hello: String,
                    accounts: [Account]
            }
            ", _ =>
            {
                _.Types.Include<Query>();
            });

            return myRet;
        }

        public class StarWarsDB
        {
            public static IEnumerable<Account> GetJedis()
            {
                return new List<Account>() {
                    new Account(){ Name ="Luke", Side="Light"},
                    new Account(){ Name ="Yoda", Side="Light"},
                    new Account(){ Name ="Darth Vader", Side="Dark"}
            };
            }

        } // end of starwars DB class

        public class Query
        {
            [GraphQLMetadata("accounts")]
            public IEnumerable<Account> GetJedis()
            {
                return StarWarsDB.GetJedis();
            }

            [GraphQLMetadata("hello")]
            public string GetHello()
            {
                return "Hello Query class";
            }
        } // end of Query class
    } // end class Data
}
