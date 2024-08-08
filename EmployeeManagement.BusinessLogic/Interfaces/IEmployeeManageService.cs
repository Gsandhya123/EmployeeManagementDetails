using EmployeeManagement.Models;

namespace EmployeeManagement.BusinessLogic.Interfaces
{
    public interface IEmployeeManageService
    {
        List<EmployeeManageDto> GetAllEmployees();
        void AddEmployee(EmployeeManageDto employeeDto);
    }
}
