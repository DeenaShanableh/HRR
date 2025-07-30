namespace HRR.DTOs.Employee
{
    public class EmployeeDto//Data Transfer Object
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; } // Nullable // Optional
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public string? Phone { get; set; }
        public long? DepartmentId { get; set; }
        public long? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string DepartmentName { get; internal set; }
        public long? PostionId { get; set; }
        public string? PostionName { get; set; }
    }
}
