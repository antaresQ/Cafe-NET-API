using System.Data.SQLite;
using Microsoft.Extensions.Options; 

namespace Cafe_NET_API.Helper
{
    public class DbContext
    {
        private readonly SQLiteConfig _sqLiteConfig;
        private readonly SQLiteConnection _sqliteConn;

        public DbContext(IOptions<SQLiteConfig> sqLiteConfig, SQLiteConnection sqLiteConnection)
        {
            _sqLiteConfig = sqLiteConfig.Value;
            _sqliteConn = sqLiteConnection;
        }

        public async Task InitializeAsync()
        {
            await _initializeDatabase();
            await _initializeTables();
        }

        private async Task _initializeDatabase()
        {

            if (!File.Exists(_sqLiteConfig.DBPath)) 
            {
                SQLiteConnection.CreateFile(_sqLiteConfig.DBPath);
            }

        }

        private async Task _initializeTables()
        {
            // create tables if they don't exist
            await _sqliteConn.OpenAsync();

            var createEmployeeTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Employee (
                        id VARCHAR(9) PRIMARY KEY NOT NULL,
                        name VARCHAR(255),
                        email_Address VARCHAR(255),
                        phone_number INTEGER,
                        gender VARCHAR(5),
                        start_date DATE
                    );
                ";

            // For Storing GUID as Blob
            //string createCafeTableQuery = @"
            //        CREATE TABLE IF NOT EXISTS Cafe (
            //            id BLOB PRIMARY KEY NOT NULL,
            //            name VARCHAR(255),
            //            description VARCHAR(1024),
            //            logo VARCHAR(512),
            //            location VARCHAR(255)
            //        );";

            string createCafeTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Cafe (
                        id VARCHAR(36) PRIMARY KEY NOT NULL,
                        name VARCHAR(255),
                        description VARCHAR(1024),
                        logo VARCHAR(512),
                        location VARCHAR(255)
                    );";

            var createCafeEmployeeTableQuery = """
                    CREATE TABLE IF NOT EXISTS CafeEmployee (
                        id INTEGER  PRIMARY KEY AUTOINCREMENT,
                        cafe_id VARCHAR(9) NOT NULL,
                        employee_id BLOB NOT NULL UNIQUE
                    );
                """;

            using (var command = new SQLiteCommand(_sqliteConn))
            {
                command.CommandText = createEmployeeTableQuery;
                await command.ExecuteNonQueryAsync();

                command.CommandText = createCafeTableQuery;
                await command.ExecuteNonQueryAsync();

                command.CommandText = createCafeEmployeeTableQuery;
                await command.ExecuteNonQueryAsync();
            }

            await _sqliteConn.CloseAsync();
        }
    }
}
