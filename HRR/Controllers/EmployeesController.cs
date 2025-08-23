using HRR.DTOs.Employee;
using HRR.Model;

using HRR.NewFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
namespace HRR.Controllers
{
    [Authorize] //Authentication / Authorization
    [Route("api/Employees")] //-- Data Anonotation
    [ApiController] //-- Data Anonotation
    public class EmployeesController : ControllerBase

    {
        private HrDbContext _dbContext;

        public EmployeesController(HrDbContext dbContext)//Constructer
        {
            _dbContext = dbContext;
           
        }
        [Authorize(Roles = "HR,Admin")]
        [HttpGet("GetAll")] //--> Data Anonotation
        public IActionResult GetAll([FromQuery]FilterEmployeeDto filterDto)//postion Is Optional //Query Parameter
        {
            try
            {
                var data = from employee in _dbContext.Employees
                           from department in _dbContext.Departments.Where(x => x.Id == employee.DepartmentId).DefaultIfEmpty()//Left Join
                           from Manager in _dbContext.Employees.Where(x => x.Id == employee.ManagerId).DefaultIfEmpty()//Left Join
                           from lookup in _dbContext.Lookups.Where(x => x.Id == employee.PostionId).DefaultIfEmpty()//Left join
                           where
                                 (filterDto.PostionId == null || employee.PostionId == filterDto.PostionId) &&//employee.position == postion //filter
                                 (filterDto.EmployeeName == null || employee.Name.ToUpper().Contains(filterDto.EmployeeName.ToUpper())) &&
                                 (filterDto.IsActive == null || employee.IsActive == filterDto.IsActive)

                           orderby employee.Id
                           select new EmployeeDto
                           {
                               Id = employee.Id,
                               Name = employee.Name,
                               BirthDate = employee.BirthDate,
                               PostionId = employee.PostionId,
                               PostionName = lookup.Name,
                               IsActive = employee.IsActive,
                               StartDate = employee.StartDate,
                               Phone = employee.Phone,
                               ManagerId = employee.ManagerId,
                               ManagerName = Manager.Name,
                               DepartmentId = employee.DepartmentId,
                               DepartmentName = department.Name,
                           };
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            // try
            // {
            //     int x = 0;
            //     return Ok(10 / x); 
            // }
            // catch (Exception ex)
            // {
            //     return BadRequest(ex.Message);//400
            //}            
            //return NotFound("Employee Not Found");//404
            //return Ok("Employee Name : Ahmad , Salary : 1000");//200
        }

        //[HttpPost]
        //public IActionResult GetAll()
        //{
        //    return Ok("Employee Name : Ahmad , Salary : 1000");
        //}


        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery]long Id) //1
        {
            try
            {
                // var employee = employees.FirstOrDefault(x => x.Id == Id);
                var employee = _dbContext.Employees.Select(employee => new EmployeeDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    BirthDate = employee.BirthDate,
                    PostionId = employee.PostionId,
                    PostionName = employee.Lookup.Name,
                    StartDate = employee.StartDate,
                    ManagerId = employee.ManagerId,
                    DepartmentId = employee.DepartmentId,
                    DepartmentName = employee.DepartmentRow.Name,
                    ManagerName = employee.Manager.Name,
                }).FirstOrDefault(x => x.Id == Id);// 

                //var employee = employees.Where(x => x.Id == Id).Select(employee => new EmployeeDto
                //{
                //    Id = employee.Id,
                //    Name = employee.Name,
                //    Age = employee.Age,
                //    Postion = employee.Postion,
                //    StartDate = employee.StartDate,

                //}); // Method Syntax
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveEmployeeDto employeeDto)
        {
            try { 
            var user = new User()
            {
                Id = 0,
                UserName = $"{employeeDto.Name}_HR",//Ahmad -->Ahmad_HR
                HashedPassword = BCrypt.Net.BCrypt.HashPassword($"{employeeDto.Name}@123"),//Ahmad -->Ahmad@123
                IsAdmin = false
            };
                var _user = _dbContext.Users.FirstOrDefault(x => x.UserName.ToUpper() == user.UserName.ToUpper());
                if (_user != null)
                {
                    return BadRequest("cannot Add this Employee : The Username Already Exist . Please Select another name");
                }
            _dbContext.Users.Add(user);

            var employee = new Employee()
            {
                Id = 0,//Ignored
                Name = employeeDto.Name,
                BirthDate = employeeDto.BirthDate,
                PostionId = employeeDto.PostionId,
                IsActive = employeeDto.IsActive,
                StartDate = employeeDto.StartDate,
                EndDate = employeeDto.EndDate,
                DepartmentId = employeeDto.DepartmentId,
                ManagerId = employeeDto.ManagerId,
                User = user


            };

            _dbContext.Employees.Add(employee);

            _dbContext.SaveChanges();
            return Ok();
        }
             catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet("Example")]
        //public IActionResult Example()
        //{
        //    var temp =  employees.LastOrDefault().Id + 1;
        //    return Ok(temp);

        //}
        [HttpPut("Update")]
        public IActionResult Update([FromBody]SaveEmployeeDto employeeDto)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.Id); // Employee to be updated
                if (employee == null)
                {
                    return BadRequest("Employee Not Found");//400
                }
                employee.Name = employeeDto.Name;
                employee.BirthDate = employeeDto.BirthDate;
                employee.PostionId = employeeDto.PostionId;
                employee.IsActive = employeeDto.IsActive;
                employee.StartDate = employeeDto.StartDate;
                employee.EndDate = employeeDto.EndDate;
                employee.DepartmentId = employeeDto.DepartmentId;
                employee.ManagerId = employeeDto.ManagerId;

                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery]long id) 
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                {
                    return BadRequest("Employee Not Found");//400
                }
                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


//Simple Data Type : long, int, string.....|Quere Parameter
//Complex Data Type : Model , Dto (Object) |Request Body
//Http Get : Can Not Use Body Request [FromBody] , We Can Only Use Query Parameter [FromeQuery]
//Http Put/Post : Can Use Both Body Request [FroamBody] And Query Parameter [Faom Query], But We Will Only Use [FromeBady]
//Http Delete : Can Use Both Body Request [FroamBody] And Query Parameter [Faom Query], But We Will Only Use [FromeBady]
//Can't Use Multiple Paramter Of Type [FroamBody]
//Can't Use Multiple Paramter Of Type [FroamQuery]