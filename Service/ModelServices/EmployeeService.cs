﻿using DAL;
using Model;
using System.Security.Cryptography;
using System.Text;

namespace Service.ModelServices
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

            if (employee == null)
            {
                return null;
            }

            if (!employee.VerifyPassword(enteredPassword))
            {
                return null;
            }

            return employee;
        }

        public Employee GetEmployeeByNumber(string employeeNumber)
        {
            return _employeeDao.GetEmployeeByNumber(employeeNumber);
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

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _employeeDao.GetEmployeeByEmailAsync(email);
        }

        public async Task UpdateEmployeePasswordAsync(string employeeNumber, string password)
        {
            (string encryptedPassword, byte[] salt) = EncryptPassword(password);
            await _employeeDao.UpdateEmployeePasswordAsync(employeeNumber, encryptedPassword, salt);
        }

        public async Task WarmUp()
        {
            await _employeeDao.WarmUp();
        }
    }
}