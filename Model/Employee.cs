﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Model
{
    public class Employee(string firstName, string lastName, DateTime birthDate, string email, string phoneNumber, OccupationType occupation, string employeeNumber, string password, byte[] salt)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
        [Column(TypeName = "Date")]
        public DateTime BirthDate { get; private set; } = birthDate;
        public string Email { get; private set; } = email;
        public string PhoneNumber { get; private set; } = phoneNumber;
        public OccupationType Occupation { get; private set; } = occupation;
        [Required]
        public string EmployeeNumber { get; private set; } = employeeNumber;

        private string _password = password;
        private byte[] _salt = salt;

        public bool VerifyPassword(string enteredPassword)
        {
            using var hmac = new HMACSHA256(_salt);
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

            return Convert.ToBase64String(hash) == _password;
        }
    }
}
