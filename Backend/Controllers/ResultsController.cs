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
    public class ResultsController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public ResultsController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetResultDTO>>> GetResults()
        {
            var results = await _context.Results
                .Select(r => new GetResultDTO
                {
                    ResultId = r.ResultId,
                    AssessmentId = r.AssessmentId,
                    UserId = r.UserId,
                    Score = r.Score,
                    AttemptDate = r.AttemptDate
                })
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/Results/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetResultDTO>> GetResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
                return NotFound();

            var dto = new GetResultDTO
            {
                ResultId = result.ResultId,
                AssessmentId = result.AssessmentId,
                UserId = result.UserId,
                Score = result.Score,
                AttemptDate = result.AttemptDate
            };

            return Ok(dto);
        }

        // POST: api/Results
        [HttpPost]
        public async Task<ActionResult<GetResultDTO>> PostResult(CreateResultDTO dto)
        {
            var result = new Result
            {
                ResultId = Guid.NewGuid(),
                AssessmentId = dto.AssessmentId,
                UserId = dto.UserId,
                Score = dto.Score,
                AttemptDate = dto.AttemptDate
            };

            _context.Results.Add(result);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResultExists(result.ResultId))
                    return Conflict();
                else
                    throw;
            }

            var getDto = new GetResultDTO
            {
                ResultId = result.ResultId,
                AssessmentId = result.AssessmentId,
                UserId = result.UserId,
                Score = result.Score,
                AttemptDate = result.AttemptDate
            };

            return CreatedAtAction(nameof(GetResult), new { id = result.ResultId }, getDto);
        }

        // PUT: api/Results/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(Guid id, PutResultDTO dto)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
                return NotFound();

            result.AssessmentId = dto.AssessmentId;
            result.UserId = dto.UserId;
            result.Score = dto.Score;
            result.AttemptDate = dto.AttemptDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Results/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null)
                return NotFound();

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultExists(Guid id)
        {
            return _context.Results.Any(e => e.ResultId == id);
        }
    }
}