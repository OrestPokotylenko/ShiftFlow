using Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class EmployeeDao
    {
        private ShiftFlowContext _context = new();

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            var foundEmployee = await _context.Employees.FindAsync(employee.Id);

            if (foundEmployee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetEmployeeByNumberAsync(string employeeNumber)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
        }
    }
}
