using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Cafe_NET_API.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Data.SQLite;

namespace Cafe_NET_API.Data
{
    public class CafeRepository : ICafeRepository
    {
        private readonly SQLiteConfig _sqliteConfig;

        public CafeRepository(IOptions<SQLiteConfig> sqliteConfig)
        {
            _sqliteConfig = sqliteConfig.Value;
        }

        public async Task<bool> CreateCafe(Cafe cafe)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                //this method is for guid saved as blob in SQlite DB
                //cafe.Id = Guid.NewGuid();
                //CafeEntity cafeE = new CafeEntity(cafe);

                //string query = string.IsNullOrEmpty(cafe.Logo) ?
                //                    @$"INSERT INTO Cafe(id, name, description, location)
                //                        VALUES(X'{cafeE.Id}', '{cafeE.Name}', '{cafeE.Description}', '{cafeE.Location}')" :
                //                    @$"INSERT INTO Cafe(id, name, description, logo, location)
                //                        VALUES(X'{cafeE.Id}', '{cafeE.Name}', '{cafeE.Description}', '{cafeE.Logo}', '{cafeE.Location}')";


                cafe.Id = Guid.NewGuid();

                string query = string.IsNullOrEmpty(cafe.Logo) ?
                                    @$"INSERT INTO Cafe(id, name, description, location)
                                    VALUES('{cafe.Id.ToSafeString().ToUpper()}', '{cafe.Name}', '{cafe.Description}', '{cafe.Location}')" :
                                    @$"INSERT INTO Cafe(id, name, description, logo, location)
                                    VALUES(X'{cafe.Id.ToSafeString().ToUpper()}', '{cafe.Name}', '{cafe.Description}', '{cafe.Logo}', '{cafe.Location}')";

                _sqliteConnection.Open();

                var outcome = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();


                return outcome == 1;
            }
        }

        public async Task<IEnumerable<Cafe>> GetCafes(string? location = null)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                //this method is for guid saved as blob in SQlite DB
                //string query = "SELECT hex(id) AS id, name, description, logo, location FROM Cafe";

                string query = "SELECT * FROM Cafe";

                if (!string.IsNullOrEmpty(location))
                {
                    query = string.Format($"{query} WHERE location = '{location}'");
                }

                _sqliteConnection.Open();

                var entityResults = _sqliteConnection.Query<CafeEntity>(query);

                _sqliteConnection.Close();

                List<Cafe> results = new List<Cafe>();

                foreach (var res in entityResults)
                {
                    results.Add(new Cafe(res));
                }

                return results;
            }
        }

        public async Task<Cafe> GetCafe(Guid id)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                //this method is for guid saved as blob in SQlite DB
                //string query = "SELECT hex(id) AS id, name, description, logo, location FROM Cafe";

                string query = @$"SELECT * FROM Cafe WHERE id = '{id.ToString().ToUpper()}'";

                _sqliteConnection.Open();

                var results = _sqliteConnection.QueryFirst<CafeEntity>(query);

                _sqliteConnection.Close();

                return new Cafe(results);
            }
        }

        public async Task<bool> UpdateCafe(Cafe cafe)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                //this method is for guid saved as blob in SQlite DB
                //string query = @$"UPDATE Cafe
                //                    SET name='{cafe.Name}', description='{cafe.Description}', logo='{cafe.Logo}', location='{cafe.Location}' 
                //                    WHERE id=X'{cafe.Id.ToHexString()}'";

                string query = @$"UPDATE Cafe
                                SET name='{cafe.Name}', description='{cafe.Description}', logo='{cafe.Logo}', location='{cafe.Location}' 
                                WHERE id='{cafe.Id.ToSafeString().ToUpper()}'";

                _sqliteConnection.Open();

                var outcome = _sqliteConnection.Execute(query);

                _sqliteConnection.Close();


                return outcome == 1;
            }
        }

        public async Task<bool> DeleteCafe(Guid id)
        {
            using (SQLiteConnection _sqliteConnection = EstablishSQLiteConnection())
            {
                //this method is for guid saved as blob in SQlite DB
                //string query = @$"DELETE FROM Cafe
                //                    WHERE id='X{id.ToHexString()}'";

                string query = @$"DELETE FROM Cafe
                                WHERE id='{id.ToSafeString().ToUpper()}'";

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
