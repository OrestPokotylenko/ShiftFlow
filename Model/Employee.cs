﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Model
{
    public class Employee(string firstName, string lastName, DateOnly birthDate, string email, string phoneNumber, string address, OccupationType occupation, DateOnly hireDate, ContractType contract, string employeeNumber, string password, byte[] salt)
    {
        public int EmployeeId { get; private set; }
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public DateOnly BirthDate { get; private set; } = birthDate;
        public string Email { get; set; } = email;
        public string PhoneNumber { get; set; } = phoneNumber;
        public string Address { get; set; } = address;
        public OccupationType Occupation { get; private set; } = occupation;
        public DateOnly HireDate { get; private set; } = hireDate;
        public ContractType Contract { get; private set; } = contract;
        public string EmployeeNumber { get; private set; } = employeeNumber;

        private string _password = password;
        private byte[] _salt = salt;

        public bool VerifyPassword(string enteredPassword)
        {
            using var hmac = new HMACSHA256(_salt);
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

            return Convert.ToBase64String(hash) == _password;
        }

        public void SetPassword(string password) => _password = password;
        public void SetSalt(byte[] salt) => _salt = salt;
    }
}
