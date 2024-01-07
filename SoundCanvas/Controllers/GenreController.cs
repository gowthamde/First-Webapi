using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoundCanvas.Data;
using SoundCanvas.DTOs;
using SoundCanvas.Models;
using System.Collections.Generic;
using System.Linq;

namespace SoundCanvas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly ApplicationDb _db;

        public GenreController(ApplicationDb db)
        {
            this._db = db;
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var genres = _db.Genres.ToList();
            var toReturn = new List<GenreDto>();
            foreach(var genre in genres)
            {
                var genreDto = new GenreDto
                {
                    id = genre.Id,
                    name = genre.Name
                };
                toReturn.Add(genreDto);
            }
            return Ok(toReturn);
        }

        [HttpGet("get-one/{id}")]
        public IActionResult Get(int id)
        {
            var genre = _db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            var toReturn = new GenreDto
            {
                id = genre.Id,
                name = genre.Name
            };
            return Ok(toReturn);
        }
        [HttpPost("create")]
        public IActionResult Create(GenreAddEditDto request)
        {
            if (!IsGenreExists(request.Name))
            {
                var genreToAdd = new Genre
                {
                    Name = request.Name.ToLower()
                };
                _db.Genres.Add(genreToAdd);
                _db.SaveChanges();
                return NoContent();
            }
            return BadRequest("Cant create duplicate genre");
        }

        [HttpPut("update")]
        public IActionResult Update(GenreAddEditDto request)
        {
            var genre = _db.Genres.Find(request.Id);
            if (genre == null)
            {
                return NotFound();

            }
            if (IsGenreExists(request.Name))
            {
                return BadRequest("Cant create duplicate genre");
            }
            genre.Name = request.Name.ToLower();
            _db.SaveChanges();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult delete(int id)
        {
            var genre = _db.Genres.Find(id);
                if (genre == null)
            {
                return NotFound();
            }
            _db.Genres.Remove(genre);
            _db.SaveChanges();
            return NoContent();
        }
            public bool IsGenreExists(string name)
        {
            var isExists = _db.Genres.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (isExists == null)
            {
                return false;
            }
            return true;
        }
    }
}
