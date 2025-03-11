using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
	public class Employee
	{
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Provide valid Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Provide valid Department")]
        public string Department { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Please Provide Email")]
        public string Email { get; set; }

        public Employee()
            {
            }

        public Employee(int id, string name, string department, string email)
            {
            Id = new Random().Next(1000, 9999);
            Name = name;
            Department = department;
            Email = email;
            }
        }
}