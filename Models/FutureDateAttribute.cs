using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (((DateTime)value) <= DateTime.Today)
            {
                return new ValidationResult("Only dates in the future are allowed!");
            }
            return ValidationResult.Success;
        }
    }
