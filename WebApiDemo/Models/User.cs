using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApiDemo.Enums;
namespace WebApiDemo.Models
{
	public class User
	{
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRole userRole { get; set; }

        public User() { }

        public User(int id, string email, string password, UserRole userRole)
            {
            Id = new Random().Next(1000, 9999);
            Email = email;
            Password = password;
            this.userRole = userRole;
            }
        }
}