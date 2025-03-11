using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
	public class EmployeeContext : DbContext
        {
        public DbSet<Employee> Employees { get; set; }
        public EmployeeContext() : base("name=EmployeeDBConnection")
            {
            }
        }
}