using HRR.DTOs.Employee;
using HRR.Model;

using HRR.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
namespace HRR.Controllers
{
    [Route("api/Employees")] //-- Data Anonotation
    [ApiController] //-- Data Anonotation
    public class EmployeesController : ControllerBase

    {
        private HrDbContext _dbContext;

        //NuGet Package -->Library
        //public static List<Employee> employees = new List<Employee>()
        //{
        //    new Employee {Id = 1 ,Name = "employee1",Age = 25,Postion ="Developer", IsActive = true ,StartDate = new DateTime(2025,1,25)},
        //    new Employee {Id = 2 ,Name = "employee1",Age = 25,Postion ="Developer", IsActive = true ,StartDate = new DateTime(2025,1,25)},
        //    new Employee {Id = 3 ,Name = "employee3",Age = 25,Postion ="Hr", IsActive = true ,StartDate = new DateTime(2025,9,10)}
        //};
        public EmployeesController(HrDbContext dbContext)//Constructer
        {
            _dbContext = dbContext;
            //employees.Add(new Employee { Name = "employee1", Age = 24, Postion = "Developer" });
            //employees.Add(new Employee { Name = "employee2", Age = 59, Postion = "Developer" });
            //employees.Add(new Employee { Name = "employee3", Age = 30, Postion = "HR" });
            //employees.Add(new Employee { Name = "employee4", Age = 20, Postion = "Manger" });
           
        }

        [HttpGet("GetAll")] //--> Data Anonotation
        public IActionResult GetAll([FromQuery]FilterEmployeeDto filterDto)//postion Is Optional //Query Parameter
        {
            var data = from employee in _dbContext.Employees
                       from department in _dbContext.Departments.Where(x => x.Id == employee.DepartmentId).DefaultIfEmpty()//Left Join
                       from Manager in _dbContext.Employees.Where(x => x.Id == employee.ManagerId).DefaultIfEmpty()//Left Join
                       from lookup in _dbContext.Lookups.Where(x => x.Id == employee.PositionId).DefaultIfEmpty()//Left join
                       where 
                             (filterDto.PositionId == null|| employee.PositionId == filterDto.PositionId )&&//employee.position == postion //filter
                             (filterDto.EmployeeName == null || employee.Name.ToUpper().Contains(filterDto.EmployeeName.ToUpper()))&&
                             (filterDto.IsActive == null || employee.IsActive == filterDto.IsActive)

                       orderby employee.Id
                            select new EmployeeDto
                            {
                                Id = employee.Id,
                                Name = employee.Name,
                                BirthDate = employee.BirthDate,
                                PositionId = employee.PositionId,
                                PositionName = lookup.Name,
                                IsActive = employee.IsActive,
                                StartDate = employee.StartDate,
                                Phone = employee.Phone,
                                ManagerId = employee.ManagerId,              
                                ManagerName = Manager.Name,
                                DepartmentId = employee.DepartmentId,
                            };
            return Ok(data);
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
           // var employee = employees.FirstOrDefault(x => x.Id == Id);
            var employee =_dbContext.Employees.Select(employee => new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                BirthDate = employee.BirthDate,
                PositionId = employee.PositionId,
                PositionName = employee.Lookup.Name,
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
        [HttpPost("Add")]
        public IActionResult Add([FromBody]SaveEmployeeDto employeeDto,[FromQuery]long example)
        {
            var employee = new Employee()
            {
                Id = 0,//Ignored
                Name = employeeDto.Name,
                BirthDate = employeeDto.BirthDate,
                PositionId = employeeDto.PositionId,
                IsActive = employeeDto.IsActive,
                StartDate = employeeDto.StartDate,
                EndDate = employeeDto.EndDate,
                DepartmentId = employeeDto.DepartmentId,
                ManagerId = employeeDto.ManagerId

            };
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
            return Ok();
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
            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.Id); // Employee to be updated
            if(employee == null)
            {
                return BadRequest("Employee Not Found");//400
            }
            employee.Name = employeeDto.Name;
            employee.BirthDate = employeeDto.BirthDate;
            employee.PositionId = employeeDto.PositionId;
            employee.IsActive = employeeDto.IsActive;
            employee.StartDate = employeeDto.StartDate;
            employee.EndDate = employeeDto.EndDate;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.ManagerId = employeeDto.ManagerId;

            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery]long id) 
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
    }
}


//Simple Data Type : long, int, string.....|Quere Parameter
//Complex Data Type : Model , Dto (Object) |Request Body
//Http Get : Can Not Use Body Request [FromBody] , We Can Only Use Query Parameter [FromeQuery]
//Http Put/Post : Can Use Both Body Request [FroamBody] And Query Parameter [Faom Query], But We Will Only Use [FromeBady]
//Http Delete : Can Use Both Body Request [FroamBody] And Query Parameter [Faom Query], But We Will Only Use [FromeBady]
//Can't Use Multiple Paramter Of Type [FroamBody]
//Can't Use Multiple Paramter Of Type [FroamQuery]