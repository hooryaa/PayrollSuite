namespace PayrollApi.DTOs
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public int? DepartmentID { get; set; }
        public int? PositionID { get; set; }
        public string EmploymentType { get; set; } = "FullTime";
    }
}
