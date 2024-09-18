using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Cafe_NET_API.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace Cafe_NET_API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SQLiteConfig _sqliteConfig;

        public EmployeeRepository(IOptions<SQLiteConfig> sqliteConfig)
        {
            _sqliteConfig = sqliteConfig.Value;
        }

        public async Task<string> CreateEmployee(Employee employee)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string id = $"UI{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";

                string query = @$"INSERT INTO Employee(id, name, email_address, phone_number, gender, start_date)
                                VALUES('{id}', '{employee.Name}', '{employee.Email_Address}', '{employee.Phone_Number}', '{employee.Gender.ToString()}', '{employee.Start_Date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}')";

                _sqliteConnection.Open();

                var outcome = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();


                return outcome == 1 ? id : string.Empty;
            }
        }

        public async Task<IEnumerable<EmployeeDetail>> GetEmployees()
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = $@"SELECT e.id, e.name, e.email_Address, e.phone_number, e.email_Address, e.start_date, c.name as Cafe
                                FROM CafeEmployee ce
                                INNER JOIN Cafe c
                                ON ce.cafe_id = c.id
                                INNER JOIN Employee e
                                ON ce.employee_id = e.id";

                _sqliteConnection.Open();

                var results = _sqliteConnection.Query<EmployeeDetail>(query);

                _sqliteConnection.Close();


                return results;
            }
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = @$"UPDATE Employee
                                SET name='{employee.Name}', email_address='{employee.Email_Address}', phone_number={employee.Phone_Number}, gender='{employee.Gender}', start_date='{employee.Start_Date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}'
                                WHERE id='{employee.Id}'";

                _sqliteConnection.Open();

                var outcome = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();


                return outcome == 1;
            }
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = @$"DELETE FROM Employee
                                WHERE id='{id}'";

                _sqliteConnection.Open();

                var outcome = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();


                return outcome == 1;
            }
        }

        private SQLiteConnection EstablishSQLiteConnection()
        {
            return new SQLiteConnection($"Data Source={_sqliteConfig.DBPath};Version={_sqliteConfig.Version}");
        }
    }
}
