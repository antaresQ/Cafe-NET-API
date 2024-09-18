using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Dapper;
using System.Data.SQLite;

namespace Cafe_NET_API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SQLiteConnection _sqliteConnection;

        public EmployeeRepository(SQLiteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
        }

        public async Task<string> CreateEmployee(Employee employee)
        {
            string id = $"UI{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";

            string query = @$"INSERT INTO Employee(id, name, email_address, phone_number, gender, start_date)
                                VALUES('{id}', '{employee.Name}', '{employee.Email_Address}', '{employee.Phone_Number}', '{employee.Gender.ToString()}', '{employee.Start_Date.ToString("YYYY-MM-DD HH:mm:ss.sss")}')";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1 ? id : string.Empty;
        }

        public async Task<IEnumerable<EmployeeDetail>> GetEmployees()
        {
            string query = $@"SELECT e.id, e.name, e.email_Address, e.phone_number, e.email_Address, e.start_date, c.name as Cafe
                                FROM CafeEmployee ce
                                INNER JOIN Cafe c
                                ON ce.cafe_id = c.id
                                INNER JOIN Employee e
                                ON ce.employee_id = e.id";

            await _sqliteConnection.OpenAsync();

            var results = await _sqliteConnection.QueryAsync<EmployeeDetail>(query);

            await _sqliteConnection.CloseAsync();
            

            return results;

        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            string query = @$"UPDATE Employee
                                SET name='{employee.Name}', email_address='{employee.Email_Address}', phone_number={employee.Phone_Number}, gender='{employee.Gender}', start_date='{employee.Start_Date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}'
                                WHERE id='{employee.Id}'";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1;
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            string query = @$"DELETE FROM Employee
                                WHERE id='{id}'";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1;
        }
    }
}
