
using HRR.DTOs.Department;
using HRR.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
    

        private HrDbContext _dbContext;
        public DepartmentsController(HrDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] long Id)
        {

            var departmet = _dbContext.Departments.Select(x => new DepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                FloorNumber = x.FloorNumber
            }).FirstOrDefault(x => x.Id == Id);
            return Ok(departmet);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveDepartmetDto departmentDto)
        {
            var department = new Department
            {
                Id = 0,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                FloorNumber = departmentDto.FloorNumber
            };
            _dbContext.Departments.Add(department);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveDepartmetDto departmetDto)
        {
            var department = _dbContext.Departments.FirstOrDefault(x => x.Id == departmetDto.Id);

            if (department == null)
            {
                return BadRequest("Department Does Not Exist");
            }

            department.Name = departmetDto.Name;
            department.Description = departmetDto.Description;
            department.FloorNumber = departmetDto.FloorNumber;

            _dbContext.SaveChanges();
            return Ok(); //200

        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long Id)
        {
            var department = _dbContext.Departments.FirstOrDefault(x => x.Id == Id);

            if (department == null)
            {
                return BadRequest("Department Does Not Exist");
            }
            _dbContext.Departments.Remove(department);
            _dbContext.SaveChanges();

            return Ok();

        }
    }
}
