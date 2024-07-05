using Microsoft.EntityFrameworkCore;
using Model;

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
            var foundEmployee = await _context.Employees.FindAsync(employee.EmployeeId);

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

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task UpdateEmployeePasswordAsync(string employeeNumber, string password, byte[] salt)
        {
            var employee = await GetEmployeeByNumberAsync(employeeNumber);

            if (employee != null)
            {
                employee.SetPassword(password);
                employee.SetSalt(salt);
                await _context.SaveChangesAsync();
            }
        }
    }
}
