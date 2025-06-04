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
    public class AssessmentsController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public AssessmentsController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Assessments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAssessmentDTO>>> GetAssessments()
        {
            var assessments = await _context.Assessments
                .Select(a => new GetAssessmentDTO
                {
                    AssessmentId = a.AssessmentId,
                    CourseId = a.CourseId,
                    Title = a.Title,
                    Questions = a.Questions,
                    MaxScore = a.MaxScore
                })
                .ToListAsync();

            return Ok(assessments);
        }

        // GET: api/Assessments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssessmentDTO>> GetAssessment(Guid id)
        {
            var assessment = await _context.Assessments.FindAsync(id);

            if (assessment == null)
                return NotFound();

            var dto = new GetAssessmentDTO
            {
                AssessmentId = assessment.AssessmentId,
                CourseId = assessment.CourseId,
                Title = assessment.Title,
                Questions = assessment.Questions,
                MaxScore = assessment.MaxScore
            };

            return Ok(dto);
        }

        // POST: api/Assessments
        [HttpPost]
        public async Task<ActionResult<GetAssessmentDTO>> PostAssessment(CreateAssessmentDTO dto)
        {
            var assessment = new Assessment
            {
                AssessmentId = Guid.NewGuid(),
                CourseId = dto.CourseId,
                Title = dto.Title,
                Questions = dto.Questions,
                MaxScore = dto.MaxScore
            };

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            var responseDto = new GetAssessmentDTO
            {
                AssessmentId = assessment.AssessmentId,
                CourseId = assessment.CourseId,
                Title = assessment.Title,
                Questions = assessment.Questions,
                MaxScore = assessment.MaxScore
            };

            return CreatedAtAction(nameof(GetAssessment), new { id = assessment.AssessmentId }, responseDto);
        }

        // PUT: api/Assessments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessment(Guid id, PutAssessmentDTO dto)
        {
            var existingAssessment = await _context.Assessments.FindAsync(id);

            if (existingAssessment == null)
                return NotFound();

            existingAssessment.CourseId = dto.CourseId;
            existingAssessment.Title = dto.Title;
            existingAssessment.Questions = dto.Questions;
            existingAssessment.MaxScore = dto.MaxScore;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessmentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Assessments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(Guid id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
                return NotFound();

            _context.Assessments.Remove(assessment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssessmentExists(Guid id)
        {
            return _context.Assessments.Any(a => a.AssessmentId == id);
        }
    }
}
