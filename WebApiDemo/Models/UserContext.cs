using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class UserContext : DbContext
        {
        public DbSet<User> Users { get; set; }
        public UserContext() : base("name=EmployeeDBConnection") { }
        }
    }