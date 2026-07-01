namespace PayrollApp.Models
{
    public class EmployeeFilterModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
    }
}
