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
                var cafeEmployees = await _cafeEmployeeRepository.GetCafeEmployees(id);

                foreach (var cafeEmp in cafeEmployees)
                {
                    if (await _employeeRepository.DeleteEmployee(cafeEmp.Id) == false)
                    {
                        throw new Exception($"Failed to Delete Employee: {cafeEmp.Id} in Cafe_id:{id}, cafe name: {cafeEmp.Cafe}");
                    }
                }

                var isCafeDeleted =  await _cafeRepository.DeleteCafe(id);

                return isCafeDeleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        #endregion


        #region Employee Services

        public async Task<string> CreateEmployee(EmployeeCreateUpdate employee)
        {
            try
            {
                var employee_id = await _employeeRepository.CreateEmployee(employee);

                var cEId = await _cafeEmployeeRepository.CreateCafeEmployee(new CafeEmployee()
                {
                    Cafe_Id = employee.Cafe_Id,
                    Employee_Id = employee_id
                });

                if (!(cEId > 0))
                {
                    throw new Exception("Failed to Assign Employee to Cafe");
                }

                return employee_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<EmployeeDetailView>> GetEmployees(string? cafeNameOrId = null)
        {
            try
            {
                IEnumerable<EmployeeDetail> employeesDetails = new List<EmployeeDetail>();

                if (string.IsNullOrEmpty(cafeNameOrId))
                {
                    employeesDetails = await _employeeRepository.GetEmployees();
                }
                else
                {
                    if (Guid.TryParse(cafeNameOrId, out var cafe_id))
                    {
                        employeesDetails = await _cafeEmployeeRepository.GetCafeEmployees(cafe_id);
                    }
                    else
                    {
                        employeesDetails = await _cafeEmployeeRepository.GetCafeEmployees(cafeNameOrId);
                    }
                }

                IList<EmployeeDetailView> employeesDetailViews = new List<EmployeeDetailView>();

                foreach (var employee in employeesDetails) 
                {
                    employeesDetailViews.Add(new EmployeeDetailView(employee));
                }

                return employeesDetailViews;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<EmployeeDetailView>();
            }
        }

        public async Task<EmployeeDetailView> GetEmployee(string? employeeId = null)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployee(employeeId);

                return new EmployeeDetailView(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new EmployeeDetailView();
            }
        }

        public async Task<bool> UpdateEmployee(EmployeeCreateUpdate employee)
        {
            try
            {
                var isEmployeeUpdated = await _employeeRepository.UpdateEmployee(employee);
                var isCafeEmployeeDeleted = await _cafeEmployeeRepository.DeleteCafeEmployee(employee.Id);

                var newCafeEmployee = new CafeEmployee() { Cafe_Id = employee.Cafe_Id, Employee_Id = employee.Id };
                newCafeEmployee.Id = await _cafeEmployeeRepository.CreateCafeEmployee(newCafeEmployee);

                var iCafeEmployeeUpdated = newCafeEmployee.Id > 0;

                return (isEmployeeUpdated && isCafeEmployeeDeleted && iCafeEmployeeUpdated);
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
