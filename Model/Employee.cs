using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Employee(string firstName, string lastName, DateTime birthDate, string email, string phoneNumber, OccupationType occupation, string employeeNumber, string password, byte[] salt)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
        public DateTime BirthDate { get; private set; } = birthDate;
        public string Email { get; private set; } = email;
        public string PhoneNumber { get; private set; } = phoneNumber;
        public OccupationType Occupation { get; private set; } = occupation;
        public string EmployeeNumber { get; private set; } = employeeNumber;

        private string _password = password;
        private byte[] _salt = salt;
    }
}
