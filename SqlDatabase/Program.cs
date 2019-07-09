using System.Data.SqlClient;
using Newtonsoft.Json;

namespace SqlDatabase
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string connectionString = "";
            const string insertCmd = "insert into Employees (FirstName, LastName) values (@FirstName, @LastName)";

            // need a connection that is initialized with a connection string
            var connection = new SqlConnection(connectionString);
            
            // sql command - sql command with the underlying connection that should be used.
            var cmd = new SqlCommand(insertCmd, connection);

            var employee = new EmployeeEntity {FirstName = "James", LastName = "Doe"};

            // parameterized
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);

            // open the connection
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close(); // close it
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