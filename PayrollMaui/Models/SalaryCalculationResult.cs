namespace PayrollMaui.Models;

public class SalaryCalculationResult
{
    public decimal BaseSalary { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetSalary { get; set; }
    public decimal Benefits { get; set; }
}
