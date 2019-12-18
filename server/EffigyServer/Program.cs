using System;
using GraphQL;
using GraphQL.Types;
using System.Resources;
using System.Collections.Generic;
using Formatters; 

namespace EffigyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Data myData = new Data();
            ISchema mySchema = myData.CreateSchema();
            string status = "";

            var root = new { Hello = "Hello World!" };
            var json = mySchema.Execute(_ =>
                {
                    _.Query = "{ hello }";
                    _.Root = root;
                });
            Console.WriteLine(json);
            Console.WriteLine(Formatters.Formatters._20Stars + "\nStart Server\n");

            gracefullyExit(status);            
        }

        public static void gracefullyExit(string status) {

            if (status == "")
                status += "No errors";

            Console.WriteLine(status);
            Console.WriteLine(Formatters.Formatters._20Stars);
        }

        //********************************************************************
        // Class Name: Data
        // Description: Holds database queries and ql schemas
        //********************************************************************
        private class Data {
            public Data(){}

            public ISchema CreateSchema() {
                var schema = Schema.For(@"
                    type Query {
                    hello: String
                    }
                ");

                return schema;
            }
        } // ennd of Data Class
    } // end of Program Class
} // end of EffigyServer Namespace
