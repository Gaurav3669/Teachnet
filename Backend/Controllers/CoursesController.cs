using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleProject.Data;
using SampleProject.Models;
using SampleProject.DTOs;

namespace SampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public CoursesController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCourseDTO>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new GetCourseDTO
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorId = c.InstructorId,
                    MediaUrl = c.MediaUrl
                })
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/Courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseDTO>> GetCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            var dto = new GetCourseDTO
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                MediaUrl = course.MediaUrl
            };

            return Ok(dto);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<GetCourseDTO>> PostCourse(CreateCourseDTO dto)
        {
            var course = new Course
            {
                CourseId = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = dto.InstructorId,
                MediaUrl = dto.MediaUrl
            };

            _context.Courses.Add(course);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.CourseId))
                    return Conflict();
                else
                    throw;
            }

            var getDto = new GetCourseDTO
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                MediaUrl = course.MediaUrl
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, getDto);
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(Guid id, PutCourseDTO dto)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.InstructorId = dto.InstructorId;
            course.MediaUrl = dto.MediaUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Courses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(Guid id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
