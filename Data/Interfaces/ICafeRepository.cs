using Cafe_NET_API.Entities;

namespace Cafe_NET_API.Data.Interfaces
{
    public interface ICafeRepository
    {
        Task<bool> CreateCafe(Cafe cafe);

        Task<IEnumerable<Cafe>> GetCafes(string? location = null);

        Task<bool> UpdateCafe(Cafe cafe);

        Task<bool> DeleteCafe(Guid id);
    }
}
