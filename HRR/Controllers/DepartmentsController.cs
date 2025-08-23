
using HRR.DTOs.Department;
using HRR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HRR.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
    

        private HrDbContext _dbContext;
        public DepartmentsController(HrDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll([FromQuery] FilterDepartmentsDto filterDto) 
        {
            try
            {
                var data = from department in _dbContext.Departments
                           from lookup in _dbContext.Lookups.Where(x => x.Id == department.TypeId).DefaultIfEmpty()
                           where (filterDto.DepartmentName == null || department.Name.ToUpper().Contains(filterDto.DepartmentName.ToUpper())) &&
                           (filterDto.FloorNumber == null || department.FloorNumber == filterDto.FloorNumber)
                           select new DepartmentDto
                           {
                               Id = department.Id,
                               Name = department.Name,
                               Description = department.Description,
                               FloorNumber = department.FloorNumber,
                               TypeId = lookup.Id,
                               TypeName = lookup.Name
                           };
                return Ok(data);
            }
            catch (Exception EX)
            {
                return BadRequest(EX.Message);
            }

        }

        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] long Id)
        {
            try
            {
                var departmet = _dbContext.Departments.Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    FloorNumber = x.FloorNumber,
                    TypeId = x.Lookup.Id,
                    TypeName = x.Lookup.Name
                }).FirstOrDefault(x => x.Id == Id);
                return Ok(departmet);
            }
            catch (Exception EX)
            {
                return BadRequest(EX.Message);
            }

   
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveDepartmetDto departmentDto)
        {
            try
            {
                var department = new Department
                {
                    Id = 0,
                    Name = departmentDto.Name,
                    Description = departmentDto.Description,
                    FloorNumber = departmentDto.FloorNumber,
                    TypeId = departmentDto.TypeId,

                };
                _dbContext.Departments.Add(department);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception EX)
            {
                return BadRequest(EX.Message);
            }

        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveDepartmetDto departmetDto)
        {
            try
            {
                var department = _dbContext.Departments.FirstOrDefault(x => x.Id == departmetDto.Id);

                if (department == null)
                {
                    return BadRequest("Department Does Not Exist");
                }

                department.Name = departmetDto.Name;
                department.Description = departmetDto.Description;
                department.FloorNumber = departmetDto.FloorNumber;
                department.TypeId = departmetDto.TypeId;
                _dbContext.SaveChanges();
                return Ok(); //200
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long Id)
        {
            try
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
            catch (Exception EX)
            {
                return BadRequest(EX.Message);
            }
        }
    }
}
