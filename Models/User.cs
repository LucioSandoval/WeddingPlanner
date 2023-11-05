using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models
{
public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [Display(Name = "First Name")]
        public string FirstName {get;set;}

        [Required]
        [Display(Name = "Last Name")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        public List<Attendance> AttendedWeddings {get;set;} //The list of Weddings that a User is attending
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "PW Confirm")]
        public string Confirm {get;set;}
    }
}  