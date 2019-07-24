using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace StorageAccount
{
    class Program
    {
        private const string PartitionKey = "class";
        private static CloudStorageAccount _storageAccount;
        private static CloudTableClient _tableClient;
        private static CloudTable _employees;

        static void Main(string[] args)
        {
            // reference to the storage account
            _storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=azureexamstorageaccount;AccountKey=g3U1JbcmBZ8c5ButInduRDWoWCTjdiYewnHH+u/kr27LX2eEao73SEmq20y15+CGVxnd84EHCEKHjY1sAKFOYw==;EndpointSuffix=core.windows.net");

            // reference to client
            _tableClient = _storageAccount.CreateCloudTableClient();

            // create a table
            _employees = _tableClient.GetTableReference("Employees");

            // employee entities to be added
            var azureEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity("David", "Ade"),
                new EmployeeEntity("Gina", "Kemi"),
                new EmployeeEntity("George", "Dredd"),
                new EmployeeEntity("Jane", "Smith")
            };

            //AddEmployeesToAzure(azureEmployees);

            var classEmployees = ReadEmployees(PartitionKey);

            Console.WriteLine("**********************************************");
            Console.WriteLine("Names of all the staff having class partition");
            Console.WriteLine("**********************************************");

            foreach (var employee in classEmployees) Console.WriteLine(employee.RowKey);

            Console.WriteLine("\n\nPress any key to exit");
            Console.ReadKey();
        }

        private static void CreateEmployees(List<EmployeeEntity> employeeEntities)
        {
            TableOperation tableOperation; // for performing table operations like insert, update

            employeeEntities.ForEach(employee =>
            {
                tableOperation = TableOperation.Insert(employee); // insert operation
                _employees.Execute(tableOperation); // actual addition into azure table
            });
        }

        private static IEnumerable<EmployeeEntity> ReadEmployees(string paritionKey)
        {
            var tableQuery = new TableQuery<EmployeeEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, paritionKey));

            return _employees.ExecuteQuery(tableQuery);
        }

        public class EmployeeEntity : TableEntity
        {
            public EmployeeEntity(string firstname, string lastname)
            {
                PartitionKey = Program.PartitionKey;
                RowKey = $"{firstname} {lastname}";
            }
            public EmployeeEntity()
            {

            }
        }
    }
}
