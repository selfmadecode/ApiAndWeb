using AutoMapper;
using BookApi.Model;
using BookApi.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Infrastructure
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseDTO>();

            CreateMap<CreateCourseDTO, Course>();

            CreateMap<UpdateCourseDTO, Course>();
        }
    }
}
