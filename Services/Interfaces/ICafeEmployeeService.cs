using Cafe_NET_API.Entities;

namespace Cafe_NET_API.Services.Interfaces
{
    public interface ICafeEmployeeService
    {
        #region Cafe Services
        Task<bool> CreateCafe(Cafe cafe);
        Task<IEnumerable<Cafe>> GetCafes(string? location = null);
        Task<bool> UpdateCafe(Cafe cafe);
        Task<bool> DeleteCafe(Guid id);
        #endregion


        #region Employee Services
        Task<string> CreateEmployee(Employee employee);
        Task<IEnumerable<EmployeeDetail>> GetEmployees(string? cafeNameOrId = null);
        Task<bool> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(string id);

        #endregion
    }
}
