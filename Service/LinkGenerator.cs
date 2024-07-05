using Model;

namespace Service
{
    public class LinkGenerator
    {
        public static DeepLink GenerateResetPasswordLink(Employee employee, out string urlSafeToken)
        {
            const string baseLink = "shiftflow://reset/";
            string employeeId = employee.EmployeeNumber.ToString();

            byte[] token = Guid.NewGuid().ToByteArray();
            string tokenString = Convert.ToBase64String(token);
            urlSafeToken = tokenString.TrimEnd('=').Replace('+', '-').Replace('/', '_');

            string link = $"{baseLink}?userId={employeeId}&token={urlSafeToken}";
            DateTime exparationTime = DateTime.Now.AddDays(1).ToUniversalTime();

            return new(employee.EmployeeId, link, exparationTime);
        }

        public static string GenerateWebLink(string token)
        {
            const string baseLink = "http://localhost:5000";
            return $"{baseLink}/?token={token}";
        }
    }
}
