namespace PayrollApp.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public int? DepartmentID { get; set; }
        public int? PositionID { get; set; }
        public string EmploymentType { get; set; } = "FullTime";
        public string? WorkEmail { get; set; }
    }
}
