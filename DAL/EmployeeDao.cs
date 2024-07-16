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

        public Employee GetEmployeeByNumber(string employeeNumber)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task UpdateEmployeePasswordAsync(Employee employee, string password, byte[] salt)
        {
            employee.SetPassword(password);
            employee.SetSalt(salt);
            await _context.SaveChangesAsync();
        }

        public async Task WarmUp()
        {
            await _context.Employees.FirstOrDefaultAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}