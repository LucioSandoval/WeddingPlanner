using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models
{


public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}

        [Required]
        [Display(Name = "Wedder One")]
        public string WedderOne {get;set;}

        [Required]
        [Display(Name = "Wedder Two")]
        public string WedderTwo {get;set;}

        [Required]
        [FutureDate]
        [Display(Name = "Date")]
        public DateTime WeddDate {get;set;}

        [Required]
        [Display(Name = "Wedding Address")]
        public string WeddAddress {get;set;}
        public int PlannerId {get;set;} //The UserId of the User (in session) who created the Wedding
        public List<Attendance> WeddingAttendees {get;set;} //The list of Users who are attending this Wedding
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}