using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Dapper;
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
            string query = @$"INSERT INTO Cafe(id, name, description, logo, location)
                                VALUES({Guid.NewGuid()}, {cafe.Name}, {cafe.Description}, {cafe.Logo}, {cafe.Location})";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();

            return outcome == 1;
        }

        public async Task<IEnumerable<Cafe>> GetCafes(string? location = null)
        {
            string query = "SELECT * FROM Cafe";

            if (!string.IsNullOrEmpty(location)) 
            {
                query = string.Format($"{query} WHERE location = {location}");
            }

            await _sqliteConnection.OpenAsync();

            var results = await _sqliteConnection.QueryAsync<Cafe>(query);

            await _sqliteConnection.CloseAsync();

            return results;
        }

        public async Task<bool> UpdateCafe(Cafe cafe)
        {
            string query = @$"UPDATE Cafe
                                SET name={cafe.Name}, description={cafe.Description}, logo={cafe.Logo}, location={cafe.Location} 
                                WHERE id={cafe.Id}";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();

            return outcome == 1;

        }

        public async Task<bool> DeleteCafe(Guid id)
        {
            string query = @$"DELETE FROM Cafe
                                WHERE id={id}";

            await _sqliteConnection.OpenAsync();

            var outcome = await _sqliteConnection.ExecuteAsync(query);

            await _sqliteConnection.CloseAsync();

            return outcome == 1;
        }
    }
}
