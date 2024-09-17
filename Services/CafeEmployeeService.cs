using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Entities;
using Cafe_NET_API.Services.Interfaces;

namespace Cafe_NET_API.Services
{
    public class CafeEmployeeService: ICafeEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeRepository _cafeRepository;
        private readonly ICafeEmployeeRepository _cafeEmployeeRepository;

        public CafeEmployeeService(IEmployeeRepository employeeRepository, ICafeRepository cafeRepository, ICafeEmployeeRepository cafeEmployeeRepository)
        {
            _employeeRepository = employeeRepository;
            _cafeRepository = cafeRepository;
            _cafeEmployeeRepository = cafeEmployeeRepository;
        }

        #region Cafe Services

        public async Task<bool> CreateCafe(Cafe cafe)
        {
            try
            {
                return await _cafeRepository.CreateCafe(cafe);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Cafe>> GetCafes(string? location = null)
        {
            try
            {
                return await _cafeRepository.GetCafes(location);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Cafe>();
            }
        }

        public async Task<bool> UpdateCafe(Cafe cafe)
        {
            try
            {
                return await _cafeRepository.UpdateCafe(cafe);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCafe(Guid id)
        {
            try
            {
                return await _cafeRepository.DeleteCafe(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        #endregion


        #region Employee Services

        public async Task<string> CreateEmployee(Employee employee)
        {
            try
            {
                return await _employeeRepository.CreateEmployee(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<EmployeeDetail>> GetEmployees(string? cafeNameOrId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cafeNameOrId))
                {
                    return await _employeeRepository.GetEmployees();
                }
                else
                {
                    if (Guid.TryParse(cafeNameOrId, out var cafe_id))
                    {
                        return await _cafeEmployeeRepository.GetCafeEmployees(cafe_id);
                    }
                    else
                    {
                        return await _cafeEmployeeRepository.GetCafeEmployees(cafeNameOrId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<EmployeeDetail>();
            }
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            try
            {
                return await _employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            try
            {
                return await _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
