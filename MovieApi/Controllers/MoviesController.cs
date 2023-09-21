using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _dbContext;

        public MoviesController(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movies>>> GetMovies()
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            return await _dbContext.Movies.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Movies>> GetMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        [HttpPost]
        public async Task<ActionResult<Movies>> CreateMovie(Movies movie)
        {
            _dbContext.Movies?.Add(movie);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromBody] Movies movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException )
            {
                if (!isMovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return NoContent();

        }
        
        private bool isMovieExists(long id)
        {
            return (_dbContext.Movies?.Any(m => m.Id == id)).GetValueOrDefault();
        }

    }
}
