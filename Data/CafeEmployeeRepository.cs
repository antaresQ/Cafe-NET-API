using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Cafe_NET_API.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace Cafe_NET_API.Data
{
    public class CafeEmployeeRepository : ICafeEmployeeRepository
    {
        private readonly SQLiteConfig _sqliteConfig;

        public CafeEmployeeRepository(IOptions<SQLiteConfig> sqliteConfig)
        {
            _sqliteConfig = sqliteConfig.Value;
        }

        public async Task<int> CreateCafeEmployee(CafeEmployee cafeEmployee)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = @$"INSERT INTO CafeEmployee(cafe_id, employee_id)
                                VALUES('{cafeEmployee.Cafe_Id.ToString().ToUpper()}', '{cafeEmployee.Employee_Id}');
                            SELECT last_insert_rowid();";

                _sqliteConnection.Open();

                var id = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();

                return id > 0 ? id : 0;
            }
        }

        public async Task<IEnumerable<EmployeeDetail>> GetCafeEmployees(Guid cafe_Id)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
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

                _sqliteConnection.Open();

                var employees = _sqliteConnection.Query<EmployeeDetail>(query);

                _sqliteConnection.Close();

                return employees;
            }
        }

        public async Task<IEnumerable<EmployeeDetail>> GetCafeEmployees(string cafeName)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = @$"SELECT e.id, e.name, e.email_Address, e.phone_number, e.email_Address, e.start_date, c.name
                                FROM CafeEmployee ce
                                INNER JOIN Cafe c
                                ON ce.cafe_id = c.id
                                INNER JOIN Employee e
                                ON ce.employee_id = e.id
                                WHERE c.name = '{cafeName}'";

                _sqliteConnection.Open();

                var employees = _sqliteConnection.Query<EmployeeDetail>(query);

                _sqliteConnection.Close();


                return employees;
            }
        }

        public async Task<bool> DeleteCafeEmployee(string employee_id)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = @$"DELETE FROM CafeEmployee
                                WHERE employee_id='{employee_id}'"; //AND cafe_id='{cafeEmployee.Cafe_Id.ToString().ToUpper()}'";

                _sqliteConnection.Open();

                var outcome = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();


                return outcome == 1;
            }
        }

        public async Task<bool> DeleteCafe(Guid cafe_id)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                string query = @$"DELETE FROM CafeEmployee
                                WHERE cafe_id='{cafe_id.ToString().ToUpper()}'";

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
