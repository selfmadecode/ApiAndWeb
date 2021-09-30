using AutoMapper;
using BookApi.Model;
using BookApi.Model.DTO;
using BookApi.Model.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Infrastructure
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            CreateMap<Teacher, TeacherDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(s => s.Age, opt => opt.MapFrom(s => s.DateOfBirth.GetCurrentAge()));

            CreateMap<CreateTeacherDTO, Teacher>();
        }
    }
}
