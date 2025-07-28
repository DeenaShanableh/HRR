using HRR.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRR.NewFolder
{
    public class Employee
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; } // Nullable // Optional
        [MaxLength(50)]
        public string? Phone { get; set; }// Nullable // Optional
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }// Nullable // Optional

        [ForeignKey("DepartmentRow")]//forinkiy
        public long? DepartmentId { get; set; }
        public Department? DepartmentRow { get; set; }//Navigation Proprty

        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        [ForeignKey("Lookup")]
        public long? PostionId { get; set; }
        public Lookup? Lookup { get; set; }
        public string PositionName { get;  set; }
        public long? PositionId { get; set; }
    }
}
