using DAL;
using Model;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class EmployeeService
    {
        private EmployeeDao _employeeDao = new();

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _employeeDao.AddEmployeeAsync(employee);
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            await _employeeDao.DeleteEmployeeAsync(employee);
        }

        public async Task<Employee> GetEmployeeAsync(string employeeNumber, string enteredPassword)
        {
            Employee employee = await _employeeDao.GetEmployeeByNumberAsync(employeeNumber);

            if (employee == null && !employee.VerifyPassword(enteredPassword))
            {
                return null;
            }

            return employee;
        }

        public (string encryptedPassword, byte[] salt) EncryptPassword(string password)
        {
            byte[] salt;

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt = new byte[16]);
            }

            using var hmac = new HMACSHA256(salt);
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return (Convert.ToBase64String(hash), salt);
        }
    }
}
