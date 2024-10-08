﻿using Cafe_NET_API.Entities;

namespace Cafe_NET_API.Services.Interfaces
{
    public interface ICafeEmployeeService
    {
        #region Cafe Services
        Task<bool> CreateCafe(Cafe cafe);
        Task<IEnumerable<Cafe>> GetCafes(string? location = null);
        Task<Cafe> GetCafe(Guid cafe_id);
        Task<bool> UpdateCafe(Cafe cafe);
        Task<bool> DeleteCafe(Guid id);
        #endregion


        #region Employee Services
        Task<string> CreateEmployee(EmployeeCreateUpdate employee);
        Task<IEnumerable<EmployeeDetailView>> GetEmployees(string? cafeNameOrId = null);
        Task<EmployeeDetailView> GetEmployee(string? employeeId = null);
        Task<bool> UpdateEmployee(EmployeeCreateUpdate employee);
        Task<bool> DeleteEmployee(string id);

        #endregion
    }
}
