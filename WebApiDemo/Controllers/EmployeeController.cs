using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApiDemo.Utility;
using WebApiDemo.Models;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
namespace WebApiDemo.Controllers
{
    //we cannot diretly provide messagw inside Unauthorised
    //[Authorize(Roles = "Admin,User")]
    [RoutePrefix("api/employee")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
        {
        private bool IsAuthenticUser()
            {
            var AuthorizationHeader = Request.Headers.Authorization;
            if (AuthorizationHeader == null || string.IsNullOrEmpty(AuthorizationHeader.Parameter))
                {
                return false;
                }
            var token = AuthorizationHeader.Parameter;
            var Principal = JwtTokenManager.ValidateToken(token);

            return Principal != null;
            }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("login")]
        public IHttpActionResult Login(User user)
            {
            if (ModelState.IsValid)
                {
                using (var userContext = new UserContext())
                    {
                    User u = userContext.Users.FirstOrDefault(e => e.Email.Equals(user.Email) && e.Password.Equals(user.Password));
                    if (u != null)
                        {

                        return Ok(new { token = JwtTokenManager.GenerateToken(user.Email) });
                        }
                    else
                        {
                        return Content(HttpStatusCode.Unauthorized, new { message = "Not Authorized" });
                        }
                    }

                }
            else
                {
                return BadRequest("Invalid Data");
                }
            }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("register")]
        public IHttpActionResult Register(User user)
            {
            if (ModelState.IsValid)
                {
                using (var userContext = new UserContext())
                    {
                    userContext.Users.Add(user);
                    userContext.SaveChanges();
                    }
                return Ok(new { message = "Successful" });
                }
            else
                {
                return BadRequest("Invalid Data");
                }
            }

        // GET: api/employee/get-all-employees
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("get-all-employees")]

        public IHttpActionResult GetAllEmployees(int pageNumber, int limit)
            {
            using (var empContext = new EmployeeContext())
                {
                var allEmployees = empContext.Employees.OrderBy(e => e.Id)
                    .Skip((pageNumber - 1) * limit)
                    .Take(limit)
                    .ToList();
                if (allEmployees == null || !allEmployees.Any())
                    {
                    return NotFound(); // Employee data not found
                    }
                return Ok(allEmployees); // Return 200 with list of employees
                }
            }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("count")]
        public IHttpActionResult getCountOfData()
            {
            int count = 0;
            using (var empContext = new EmployeeContext())
                {
                count = empContext.Employees.Count();
                }
            return Ok(new { count = count });
            }

        // POST: api/employee/create-employee
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("create-employee")]
        public IHttpActionResult Create(Employee employee)
            {
            if (!IsAuthenticUser())
                {
                // Return 401 Unauthorized with custom message
                return Content(HttpStatusCode.Unauthorized, new { message = "You are not authorized" });
                }
            else
                {
                if (!ModelState.IsValid)
                    {
                    return BadRequest("Invalid data provided."); // 400 Bad Request for invalid data
                    }

                using (var empContext = new EmployeeContext())
                    {
                    empContext.Employees.Add(employee);
                    empContext.SaveChanges();
                    }

                return Ok(new { message = "Employee Created Successfully" });
                }
            }

        // PUT: api/employee/edit-employee/{id}
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("edit-employee/{id}")]
        public IHttpActionResult Edit(int id, Employee employee)
            {       
            if (!IsAuthenticUser())
                {
                // Return 401 Unauthorized with custom message
                return Content(HttpStatusCode.Unauthorized, new { message = "You are not authorized" });
                }

            else
                {
                if (!ModelState.IsValid)
                    {
                    return BadRequest("Invalid data provided."); // 400 Bad Request for invalid data
                    }

                using (var empContext = new EmployeeContext())
                    {
                    var emp = empContext.Employees.FirstOrDefault(e => e.Id == id);
                    if (emp == null)
                        {
                        return NotFound(); // Employee not found
                        }

                    emp.Department = employee.Department;
                    emp.Email = employee.Email;
                    emp.Name = employee.Name;
                    empContext.SaveChanges();
                    }

                return Ok(new { message = "Employee details updated successfully." }); // 200 OK on successful update
                }
            }

        // DELETE: api/employee/delete/{id}
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("delete-employee/{id}")]
        public IHttpActionResult Delete(int id)
            {
            if (!IsAuthenticUser())
                {   
                // Return 401 Unauthorized with custom message
                return Content(HttpStatusCode.Unauthorized, new { message = "You are not authorized" });
                }
            else
                {
                using (var empContext = new EmployeeContext())
                    {
                    var emp = empContext.Employees.FirstOrDefault(e => e.Id == id);
                    if (emp == null)
                        {
                        return NotFound(); // Employee not found
                        }

                    empContext.Employees.Remove(emp);
                    empContext.SaveChanges();
                    }

                return Ok(new { message = "Employee deleted successfully." }); // 200 OK on successful deletion
                }
            }
        }
    }