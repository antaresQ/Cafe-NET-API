using System.Data.SQLite;
using Microsoft.Extensions.Options; 

namespace Cafe_NET_API.Helper
{
    public class DbContext
    {
        private readonly SQLiteConfig _sqLiteConfig;

        public DbContext(IOptions<SQLiteConfig> sqLiteConfig)
        {
            _sqLiteConfig = sqLiteConfig.Value;
        }

        public async Task InitializeAsync()
        {
            await _initializeDatabase();
            await _initializeTables();
        }

        private SQLiteConnection GetSQLiteConnection()
        {
            return new SQLiteConnection($"Data Source={_sqLiteConfig.DBPath};Version={_sqLiteConfig.Version}");
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
            using (var sqLiteConn = GetSQLiteConnection())
            {
                sqLiteConn.Open();

                string createCafeTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Cafe (
                            id VARCHAR(9) PRIMARY KEY NOT NULL,
                            name VARCHAR(255),
                            description VARCHAR(1024),
                            logo VARCHAR(512),
                            location VARCHAR(255)
                        );";

                var createEmployeeTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Employee (
                            id BLOB PRIMARY KEY NOT NULL,
                            name VARCHAR(255),
                            email_Address VARCHAR(255),
                            phone_number INTEGER,
                            gender VARCHAR(5),
                            start_date DATE
                        );
                    ";

                var createCafeEmployeeTableQuery = """
                        CREATE TABLE IF NOT EXISTS CafeEmployee (
                            id INTEGER  PRIMARY KEY AUTOINCREMENT,
                            cafe_id VARCHAR(9) NOT NULL,
                            employee_id BLOB NOT NULL UNIQUE
                        );
                    """;

                using (var command = new SQLiteCommand(sqLiteConn))
                {
                    command.CommandText = createCafeTableQuery;
                    command.ExecuteNonQuery();

                    command.CommandText = createEmployeeTableQuery;
                    command.ExecuteNonQuery();

                    command.CommandText = createCafeEmployeeTableQuery;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
