using AutoMapper;
using BookApi.Model;
using BookApi.Model.DTO;
using BookApi.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    //[Route("api/teachers/{teacherId}/courses")]
    [Route("api/v{version:apiVersion}/teachers/{teacherId}/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
        }

        [HttpGet(Name = "GetAuthorCourses")]
        public ActionResult<IEnumerable<CourseDTO>> GetAuthorCourses(Guid teacherId)
        {

            if (!_repository.TeacherExists(teacherId))
                return NotFound();

            var courses = _repository.GetCourses(teacherId);

            return Ok(_mapper.Map<IEnumerable<CourseDTO>>(courses));
        }

        [HttpGet("{courseId}", Name = "GetAuthorCourse")]
        public ActionResult<CourseDTO> GetAuthorCourse(Guid courseId, Guid teacherId)
        {
            if (!_repository.TeacherExists(teacherId))
                return NotFound();

            var course = _repository.GetCourse(teacherId, courseId);

            if (course == null)
                return NotFound();

            return Ok(_mapper.Map<CourseDTO>(course));
        }

        [HttpPost(Name = "CreateCourseForTeacher")]
        public IActionResult CreateCourseForTeacher(Guid teacherId,
            CreateCourseDTO course)
        {
            if (!_repository.TeacherExists(teacherId))
                return NotFound();
            
            
            var newCourse = _mapper.Map<Course>(course);
            _repository.AddCourse(teacherId, newCourse);
            _repository.Save();

            var courseToReturn = _mapper.Map<CourseDTO>(newCourse);

            return CreatedAtRoute("GetAuthorCourse",
                new { courseId = courseToReturn.Id, teacherId }, courseToReturn);
        }

        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForTeacher(Guid teacherId, Guid courseId,
            UpdateCourseDTO course)
        {
            if (!_repository.TeacherExists(teacherId))
                return NotFound();

            var courseFromRepo = _repository.GetCourse(teacherId, courseId);

            if (courseFromRepo == null)
            {
                //upserting - create a resource is the resource dosent exist

                var newCourse = _mapper.Map<Course>(course);
                newCourse.Id = courseId;
                _repository.AddCourse(teacherId, newCourse);
                _repository.Save();

                var courseToReturn = _mapper.Map<CourseDTO>(newCourse);

                return CreatedAtRoute("GetAuthorCourse", new {
                    courseId = courseToReturn.Id,
                    teacherId }, courseToReturn);
            }

            _mapper.Map(course, courseFromRepo);
            _repository.UpdateCourse(courseFromRepo);
            _repository.Save();

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public IActionResult DeleteCourseForTeacher(Guid teacherId, Guid courseId)
        {
            if (!_repository.TeacherExists(teacherId))
                return NotFound();

            var courseFromRepo = _repository.GetCourse(teacherId, courseId);

            if (courseFromRepo == null)
            {
                return NotFound();                
            }

            _repository.DeleteCourse(courseFromRepo);
            _repository.Save();
            return NoContent();
        }
    }
}
