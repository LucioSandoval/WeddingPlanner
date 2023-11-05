using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models
{
    public class IndexViewModel
    {
        public LoginUser LogUser {get;set;}
        public User RegUser {get;set;}
    }
}
