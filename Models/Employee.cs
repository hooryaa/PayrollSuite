using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApi.Models
{
    [Table("Employees", Schema = "dbo")]
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required, MaxLength(20)]
        public string EmployeeNumber { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(1)]
        public string? Gender { get; set; }

        [MaxLength(50)]
        public string? NationalID { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int? DepartmentID { get; set; }

        public int? PositionID { get; set; }

        [MaxLength(20)]
        public string EmploymentType { get; set; } = "FullTime";

        [MaxLength(255)]
        public string? WorkEmail { get; set; }

        [MaxLength(100)]
        public string? BankAccount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
