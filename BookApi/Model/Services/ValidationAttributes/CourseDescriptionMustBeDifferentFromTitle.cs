using BookApi.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Services.ValidationAttributes
{
    public class CourseDescriptionMustBeDifferentFromTitle : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var course = (CourseManipulationDTO)validationContext.ObjectInstance;

            if (course.Title == course.Description)
            {
                return new ValidationResult("The description should be different from the title",
                    new[] {nameof(CourseManipulationDTO) });
            }

            return ValidationResult.Success;
        }
    }
}
