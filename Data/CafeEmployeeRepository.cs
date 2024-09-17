using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Cafe_NET_API.Helper;
using Dapper;
using System.Data.SQLite;

namespace Cafe_NET_API.Data
{
    public class CafeEmployeeRepository : ICafeEmployeeRepository
    {
        private readonly SQLiteConnection _sqliteConnection;

        public CafeEmployeeRepository(SQLiteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
        }

        public async Task<int> CreateCafeEmployee(CafeEmployee cafeEmployee)
        {
            string query = @$"INSERT INTO CafeEmployee(cafe_id, employee_id)
                                VALUES('{cafeEmployee.Cafe_Id.ToString().ToUpper()}', '{cafeEmployee.Employee_Id}');
                            SELECT last_insert_rowid();";

            await _sqliteConnection.OpenAsync();

            var id = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();

            return id > 0 ? id : 0;
        }

        public async Task<IEnumerable<EmployeeDetail>> GetCafeEmployees(Guid cafe_Id)
        {
            //this method is for guid saved as blob in SQlite DB
            //string query = @$"SELECT e.id, e.name, e.email_Address, e.phone_number, e.email_Address, e.start_date, c.name
            //                    FROM CafeEmployee ce
            //                    INNER JOIN Cafe c
            //                    ON ce.cafe_id = c.id
            //                    INNER JOIN Employee e
            //                    ON ce.employee_id = e.id
            //                    WHERE ce.cafe_id = 'X{cafe_Id.ToHexString()}'";

            string query = @$"SELECT e.id, e.name, e.email_Address, e.phone_number, e.email_Address, e.start_date, c.name AS cafe
                                FROM CafeEmployee ce
                                INNER JOIN Cafe c
                                ON ce.cafe_id = c.id
                                INNER JOIN Employee e
                                ON ce.employee_id = e.id
                                WHERE ce.cafe_id = '{cafe_Id.ToString().ToUpper()}'";

            await _sqliteConnection.OpenAsync();

            var employees = await _sqliteConnection.QueryAsync<EmployeeDetail>(query);

            await _sqliteConnection.CloseAsync();

            return employees;

        }

        public async Task<IEnumerable<EmployeeDetail>> GetCafeEmployees(string cafeName)
        {
            string query = @$"SELECT e.id, e.name, e.email_Address, e.phone_number, e.email_Address, e.start_date, c.name
                                FROM CafeEmployee ce
                                INNER JOIN Cafe c
                                ON ce.cafe_id = c.id
                                INNER JOIN Employee e
                                ON ce.employee_id = e.id
                                WHERE c.name = '{cafeName}'";

            await _sqliteConnection.OpenAsync();

            var employees = await _sqliteConnection.QueryAsync<EmployeeDetail>(query);

            await _sqliteConnection.CloseAsync();
            

            return employees;

        }

        public async Task<bool> DeleteCafeEmployee(string employee_id)
        {
            string query = @$"DELETE FROM CafeEmployee
                                WHERE employee_id='{employee_id}'"; //AND cafe_id='{cafeEmployee.Cafe_Id.ToString().ToUpper()}'";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1;
        }

        public async Task<bool> DeleteCafe(Guid cafe_id)
        {
            string query = @$"DELETE FROM CafeEmployee
                                WHERE cafe_id='{cafe_id.ToString().ToUpper()}'";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();

            return outcome == 1;
        }
    }
}
