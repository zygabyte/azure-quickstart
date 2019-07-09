using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CosmosDb
{
    internal class Program
    {
        private const string DatabaseName = "maindb";
        private const string ContainerName = "employee";
        private static DocumentClient _documentClient;
        
        private const string Endpoint = ""; // removed because i was pushing to a public repo
        private const string PrimaryKey = ""; // removed because i was pushing to a public repo
        
        public static void Main(string[] args)
        {
            // reference to client
            _documentClient = new DocumentClient(new Uri(Endpoint), PrimaryKey);
            
            // employee entities to be added
            var azureEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { FirstName = "David", LastName = "Ade" },
                new EmployeeEntity { FirstName = "Gina", LastName = "Kemi" },
                new EmployeeEntity { FirstName = "George", LastName = "Dredd" },
                new EmployeeEntity { FirstName = "Jane", LastName = "Smith" },
                new EmployeeEntity { FirstName = "Jane", LastName = "Doe" }
            };

//            CreateEmployees(azureEmployees);

            var readEmployees = ReadEmployees("Jane");

            Console.WriteLine("**************************************************");
            Console.WriteLine("Names of all the staff having class jane firstname");
            Console.WriteLine("**************************************************");

            foreach (var employee in readEmployees) Console.WriteLine(employee);

            Console.WriteLine("\n\nPress any key to exit");
            Console.ReadKey();
        }
        
        private static void CreateEmployees(List<EmployeeEntity> employeeEntities)
        {
            employeeEntities.ForEach(employeeEntity =>
                {
                   _documentClient.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(DatabaseName, ContainerName), employeeEntity).GetAwaiter().GetResult();
                });
        }

        private static IEnumerable<EmployeeEntity> ReadEmployees(string firstname)
        {
            var queryOptions = new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true };
     
            var employeeQuery = _documentClient
                .CreateDocumentQuery<EmployeeEntity>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, ContainerName), queryOptions)
                .Where(x => x.FirstName == firstname);

            return employeeQuery;
        }
        
        public class EmployeeEntity
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}