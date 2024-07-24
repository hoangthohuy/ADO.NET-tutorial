using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HR
{
    public class Program
    {
        static void Main(string[] args)
        {
           
            
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration configuration = builder.Build();
            
            using var conn =new SqlConnection(configuration.GetConnectionString("HRDB"));
            conn.Open();


            AddEmployee("Hoang", "Tho Huy", "hoangthohuy@gmail.com", "0971130204", DateTime.Today, 19, 100000, 100, 6, conn);
            //deleteEmployee("hoangthohuy@gmail.com",conn);
            //UpdateEmployee(101, "Huy", conn);
            conn.Close();
        }

        private static void ListCountries(SqlConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "Select * from countries";

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Console.WriteLine($"ID :{reader.GetString(0)}, Name: {reader.GetSqlString(1)}");
            }
            reader.Close();
            
        }
        private static void DisplayNumberOfCountries(SqlConnection conn)
        {
            var cmd = new SqlCommand("Select COUNT(*) from countries", conn);
            int c = (int)cmd.ExecuteScalar();
            Console.WriteLine("Total countries: {0}", c);
        }

        private static int AddEmployee(
            String firstName, String lastName, String email, String phoneNumber, DateTime hireDate, int jobId, double salary, int managerId, int departmentId, SqlConnection conn  )
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"Insert into employees values
                                        (@first_name,
                                        @last_name,
                                        @email,
                                        @phone_number,
                                        @hire_date,
                                        @job_id,
                                        @salary,
                                        @manager_id,
                                        @department_id )";

            cmd.Parameters.Add(new SqlParameter("@first_name", System.Data.SqlDbType.VarChar, 20)).Value = firstName;
            cmd.Parameters.Add(new SqlParameter("@last_name", System.Data.SqlDbType.VarChar, 25)).Value = lastName;
            cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100)).Value = email;
            cmd.Parameters.Add(new SqlParameter("@phone_number", System.Data.SqlDbType.VarChar, 20)).Value = phoneNumber;
            cmd.Parameters.Add(new SqlParameter("@hire_date", System.Data.SqlDbType.Date)).Value = hireDate;
            cmd.Parameters.Add(new SqlParameter("@job_id", System.Data.SqlDbType.Int)).Value = jobId;
            cmd.Parameters.Add(new SqlParameter("@salary", System.Data.SqlDbType.Decimal)).Value = salary;
            cmd.Parameters.Add(new SqlParameter("@manager_id", System.Data.SqlDbType.Int)).Value = managerId;
            cmd.Parameters.Add(new SqlParameter("@department_id", System.Data.SqlDbType.Int)).Value = departmentId;


            var result =  cmd.ExecuteNonQuery();
            return result;

        }

        private static void deleteEmployee(
            string email, SqlConnection conn)
        {
            var cmd= conn.CreateCommand();
            cmd.CommandText = @"Delete from employees where email = @email";

            cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100)).Value = email;

            cmd.ExecuteScalar();

        }

        private  static void UpdateEmployee(int Id,String lastName ,SqlConnection conn) 
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"Update employees set last_name = @last_name where employee_id= @employee_id";
            cmd.Parameters.Add(new SqlParameter("@employee_id", System.Data.SqlDbType.Int)).Value = Id;
            cmd.Parameters.Add(new SqlParameter("@last_name", System.Data.SqlDbType.VarChar, 25)).Value = lastName;
            cmd.ExecuteScalar();


        }
    }
}
