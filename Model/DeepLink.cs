namespace Model
{
    public class DeepLink(int employeeId, string link, DateTime expirationDate, int? deepLinkId = null)
    {
        public int? DeepLinkId { get; private set; } = deepLinkId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee? Employee { get; private set; }
        public string Link { get; private set; } = link;
        public DateTime ExpirationDate { get; set; } = expirationDate;
        public bool Used { get; set; } = false;
    }
}