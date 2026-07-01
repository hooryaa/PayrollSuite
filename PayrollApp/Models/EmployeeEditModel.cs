using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Models
{
    public class EmployeeEditModel
    {
        public int? EmployeeID { get; set; }

        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        public DateTime HireDate { get; set; } = DateTime.Today;

        public bool IsActive { get; set; } = true;
        public int? DepartmentID { get; set; }
        public int? PositionID { get; set; }
        public string EmploymentType { get; set; } = "FullTime";
        [EmailAddress]
        public string? WorkEmail { get; set; }
    }
}
