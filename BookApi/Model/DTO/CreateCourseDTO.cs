using BookApi.Model.Services.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.DTO
{
    [CourseDescriptionMustBeDifferentFromTitle]
    public class CreateCourseDTO : CourseManipulationDTO //: IValidatableObject
    {

        //inheriting from the abstarct class so as to get its members



        //[Required]
        //[MaxLength(100)]
        //public string Title { get; set; }

        //[MaxLength(1500)]
        //public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Title == Description)
        //    {
        //        yield return new ValidationResult("The description should be different from the title",
        //            new[] { "CreateCourseDTO" });
        //    }
                
        //}
    }
}
