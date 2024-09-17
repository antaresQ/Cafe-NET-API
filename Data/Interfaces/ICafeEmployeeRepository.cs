using Cafe_NET_API.Entities;

namespace Cafe_NET_API.Data.Interfaces
{
    public interface ICafeEmployeeRepository
    {
        Task<int> CreateCafeEmployee(CafeEmployee cafe);

        Task<IEnumerable<EmployeeDetail>> GetCafeEmployees(Guid cafe_Id);

        Task<IEnumerable<EmployeeDetail>> GetCafeEmployees(string cafeName);

        Task<bool> DeleteCafeEmployee(string employee_Id);

        Task<bool> DeleteCafe(Guid cafe_id);
    }
}
