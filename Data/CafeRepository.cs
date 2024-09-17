using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Cafe_NET_API.Helper;
using Dapper;
using System.Collections;
using System.Data.SQLite;

namespace Cafe_NET_API.Data
{
    public class CafeRepository : ICafeRepository
    {
        private readonly SQLiteConnection _sqliteConnection;

        public CafeRepository(SQLiteConnection sQLiteConnection)
        {
            _sqliteConnection = sQLiteConnection;
        }

        public async Task<bool> CreateCafe(Cafe cafe)
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
                                    VALUES(X'{cafe.Id}', '{cafe.Name}', '{cafe.Description}', '{cafe.Logo}', '{cafe.Location}')";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1;
        }

        public async Task<IEnumerable<Cafe>> GetCafes(string? location = null)
        {
            //this method is for guid saved as blob in SQlite DB
            //string query = "SELECT hex(id) AS id, name, description, logo, location FROM Cafe";

            string query = "SELECT * FROM Cafe";

            if (!string.IsNullOrEmpty(location)) 
            {
                query = string.Format($"{query} WHERE location = '{location}'");
            }

            await _sqliteConnection.OpenAsync();

            var entityResults = await _sqliteConnection.QueryAsync<CafeEntity>(query);

            await _sqliteConnection.CloseAsync();

            List<Cafe> results = new List<Cafe>();

            foreach (var res in entityResults) 
            { 
                results.Add(new Cafe(res));
            }

            return results;
        }

        public async Task<bool> UpdateCafe(Cafe cafe)
        {
            //this method is for guid saved as blob in SQlite DB
            //string query = @$"UPDATE Cafe
            //                    SET name='{cafe.Name}', description='{cafe.Description}', logo='{cafe.Logo}', location='{cafe.Location}' 
            //                    WHERE id=X'{cafe.Id.ToHexString()}'";

            string query = @$"UPDATE Cafe
                                SET name='{cafe.Name}', description='{cafe.Description}', logo='{cafe.Logo}', location='{cafe.Location}' 
                                WHERE id='{cafe.Id.ToSafeString().ToUpper()}'";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1;

        }

        public async Task<bool> DeleteCafe(Guid id)
        {
            //this method is for guid saved as blob in SQlite DB
            //string query = @$"DELETE FROM Cafe
            //                    WHERE id='X{id.ToHexString()}'";

            string query = @$"DELETE FROM Cafe
                                WHERE id='{id.ToSafeString()}'";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();
            

            return outcome == 1;
        }
    }
}
