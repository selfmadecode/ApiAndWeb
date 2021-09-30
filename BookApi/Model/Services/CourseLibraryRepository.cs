using System;
using System.Collections.Generic;
using System.Linq;
using BookApi.Model.Entities;
using BookApi.Model.Interfaces;
using BookApi.Model.Services.Helpers;
using BookApi.Model.ViewModels;


namespace BookApi.Model.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public CourseLibraryRepository(ApplicationDbContext context )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(Guid teacherId, Course course)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            // always set the AuthorId to the passed-in authorId
            course.TeacherId = teacherId;
            _context.Courses.Add(course); 
        }         

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }
  
        public Course GetCourse(Guid teacherId, Guid courseId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
              .Where(c => c.TeacherId == teacherId && c.Id == courseId).FirstOrDefault();
        }

        public IEnumerable<Course> GetCourses(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return _context.Courses
                        .Where(c => c.TeacherId == teacherId)
                        .OrderBy(c => c.Title).ToList();
        }

        public void UpdateCourse(Course course)
        {
            // no code in this implementation
        }

        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }

            // the repository fills the id (instead of using identity columns)
            teacher.Id = Guid.NewGuid();

            foreach (var course in teacher.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Teachers.Add(teacher);
        }

        public bool TeacherExists(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return _context.Teachers.Any(a => a.Id == teacherId);
        }

        public void DeleteTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }

            _context.Teachers.Remove(teacher);
        }
        
        public Teacher GetTeacher(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return _context.Teachers.FirstOrDefault(a => a.Id == teacherId);
        }

        public PagedList<Teacher> GetTeachers(TeacherResourceParameters filters)
        {
            var teacher = _context.Teachers.AsQueryable();

            if (!string.IsNullOrEmpty(filters.SearchParam))
            {
                //trim only removes whitespace from the begining and end of a string
                //.Replace(" ", string.Empty) removes all occurence of whitespace
                var searchQuery = filters.SearchParam.Trim().ToLower();

                teacher = teacher
                    .Where(s => s.FirstName.Contains(searchQuery)
                    || s.MainCategory.Contains(searchQuery));
            }
                

            if (!string.IsNullOrEmpty(filters.OrderBy))
            {
                switch (filters.OrderBy)
                {
                    case "name_desc":
                        teacher = teacher.OrderByDescending(n => n.FirstName);
                        break;

                    case "name_ascd":
                        teacher = teacher.OrderBy(n => n.FirstName);
                        break;
                }
            }
            return PagedList<Teacher>.Create(teacher,
                filters.PageNumber, filters.PageSize);
        }
         
        public IEnumerable<Teacher> GetTeachers(IEnumerable<Guid> teacherIds)
        {
            if (teacherIds == null)
            {
                throw new ArgumentNullException(nameof(teacherIds));
            }

            return _context.Teachers.Where(a => teacherIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateTeacher(Teacher teacher)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               // dispose resources when needed
            }
        }
    }
}
