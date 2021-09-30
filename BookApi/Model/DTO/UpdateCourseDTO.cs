using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.DTO
{
    public class UpdateCourseDTO : CourseManipulationDTO
    {
        //[Required]
        //[MaxLength(100)]
        //public string Title { get; set; }

        //[MaxLength(1500)]
        // public string Description { get; set; }

        [Required(ErrorMessage ="New validation triggered during update")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
