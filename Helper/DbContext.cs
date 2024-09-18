using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Options; 

namespace Cafe_NET_API.Helper
{
    public class DbContext
    {
        private readonly SQLiteConfig _sqliteConfig;

        public DbContext(IOptions<SQLiteConfig> sqLiteConfig)
        {
            _sqliteConfig = sqLiteConfig.Value;
        }

        public async Task InitializeAsync()
        {
            await _initializeDatabase();
            await _initializeTables();
            await _initializeSeedDataAsync();
        }

        private async Task _initializeDatabase()
        {

            if (!File.Exists(_sqliteConfig.DBPath)) 
            {
                SQLiteConnection.CreateFile(_sqliteConfig.DBPath);
            }

        }

        private async Task _initializeTables()
        {
            using (SQLiteConnection _sqliteConn = EstablishSQLiteConnection())
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

                var createCafeEmployeeTableQuery = @"
                    CREATE TABLE IF NOT EXISTS CafeEmployee (
                        id INTEGER  PRIMARY KEY AUTOINCREMENT,
                        cafe_id VARCHAR(9) NOT NULL,
                        employee_id BLOB NOT NULL UNIQUE
                    );";

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

        private async Task _initializeSeedDataAsync()
        {
            using (SQLiteConnection _sqliteConn = EstablishSQLiteConnection())
            {
                // create insert seed Data if they don't exist
                await _sqliteConn.OpenAsync();

                var checkIfCafeTableIsEmpty = @"SELECT COUNT()=0 FROM Cafe";

                var insertCafes = @"INSERT INTO 'Cafe' ('id','name','description','logo','location') 
                                    VALUES ('892101B4-BF3B-4F5B-9DA6-ACAA6E00ED95','HV Cafe','Yoghurt','','Horlan Village'),
                                             ('9FA909BA-B3BD-4FB2-9731-3FE4F6EC0944','Cat Cafe','Catastrophy',NULL,'Feli City'),
                                             ('525ADD2C-F238-42A7-A079-E8379D97E228','Nest Cafe','Coffee place','','Horlan Village'),
                                             ('642D7EEF-69E5-4B4C-98D7-6A69D95B9B51','Dog Cafe','Hot Dog Dogmatism',NULL,'Feli City'),
                                             ('1E5343FC-9D82-44E5-B42C-45F67A5BF6F7','Squirrel Cafe','Nuts only but no mutts allowed',NULL,'Timber Town')";


                var checkIfEmployeeTableIsEmpty = @"SELECT COUNT()=0 FROM Employee";

                var insertEmployees = @"INSERT INTO 'Employee' ('id','name','email_Address','phone_number','gender','start_date') 
                                        VALUES ('UICFD7927E','Hor Fun Man','man@horfun.com',91234567,'Male','2023-09-17T00:53:26.078Z'),
                                                 ('UI476E6EFA','Tabby Tan','tabby@tan.com',98765432,'Female','2023-05-01T03:13:43.146Z'),
                                                 ('UI948C603A','Chip','chip@rescuerangers.com',98785132,'Male','1991-01-01T00:00:00.000Z'),
                                                 ('UI8BB99BB4','Dale','chip@rescuerangers.com',98785133,'Male','1991-01-01T00:00:00.000Z'),
                                                 ('UI814BE4C2','Khao Manee','khao_manee@akukatsini.com',81234567,'Male','2020-03-01T03:20:18.061Z'),
                                                 ('UI9EF3AFE6','You Get Ou','you_get_ou@ofthisplace.com',80020078,'Female','2021-12-01T03:20:18.061Z'),
                                                 ('UIFD93111A','Com Onin','com_onin@andsitdown.com',90220076,'Female','2010-05-22T03:20:18.061Z')";


                var checkIfCafeEmployeeTableIsEmpty = @"SELECT COUNT()=0 FROM CafeEmployee";

                var insertCafeEmployees = @"INSERT INTO 'CafeEmployee' ('id','cafe_id','employee_id') 
                                            VALUES (6,'525ADD2C-F238-42A7-A079-E8379D97E228','UICFD7927E'),
                                                     (10,'9FA909BA-B3BD-4FB2-9731-3FE4F6EC0944','UI476E6EFA'),
                                                     (11,'1E5343FC-9D82-44E5-B42C-45F67A5BF6F7','UI948C603A'),
                                                     (12,'1E5343FC-9D82-44E5-B42C-45F67A5BF6F7','UI8BB99BB4'),
                                                     (13,'9FA909BA-B3BD-4FB2-9731-3FE4F6EC0944','UI814BE4C2'),
                                                     (14,'892101B4-BF3B-4F5B-9DA6-ACAA6E00ED95','UI9EF3AFE6'),
                                                     (15,'892101B4-BF3B-4F5B-9DA6-ACAA6E00ED95','UIFD93111A')";

                using (var command = new SQLiteCommand(_sqliteConn))
                {

                    if (IsTableEmpty(checkIfCafeTableIsEmpty) 
                        && IsTableEmpty(checkIfEmployeeTableIsEmpty)
                        && IsTableEmpty(checkIfCafeEmployeeTableIsEmpty))
                    {
                        command.CommandText = insertCafes;
                        await command.ExecuteNonQueryAsync();
                    
                        command.CommandText = insertEmployees;
                        await command.ExecuteNonQueryAsync();
                 
                        command.CommandText = insertCafeEmployees;
                        await command.ExecuteNonQueryAsync();
                    }
                }

                await _sqliteConn.CloseAsync();
            }
        }

        private bool IsTableEmpty(string query)
        {
            using (SQLiteConnection _sqliteConn = EstablishSQLiteConnection())
            {
                _sqliteConn.Open();

                var isEmpty = _sqliteConn.QueryFirst<bool>(query);

                _sqliteConn.Close();

                return isEmpty;
            }
        }

        private SQLiteConnection EstablishSQLiteConnection()
        {
            return new SQLiteConnection($"Data Source={_sqliteConfig.DBPath};Version={_sqliteConfig.Version}");
        }
    }
}
