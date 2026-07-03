using System.ComponentModel.DataAnnotations;

namespace PayrollApi.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(30)]
        public string? Phone { get; set; }

        public bool IsActive { get; set; } = true;

        public int? DepartmentID { get; set; }

        public int? PositionID { get; set; }

        [Required]
        [StringLength(20)]
        public string EmploymentType { get; set; } = "FullTime";
    }
}
