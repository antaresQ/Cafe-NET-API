using Cafe_NET_API.Entities;

namespace Cafe_NET_API.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<string> CreateEmployee(Employee employee);

        Task<IEnumerable<EmployeeDetail>> GetEmployees();

        Task<bool> UpdateEmployee(Employee employee);

        Task<bool> DeleteEmployee(string id);
    }
}
