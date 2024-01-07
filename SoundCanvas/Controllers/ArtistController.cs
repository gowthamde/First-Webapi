using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundCanvas.Data;
using SoundCanvas.DTOs;
using SoundCanvas.Models;
using System.Collections.Generic;
using System.Linq;

namespace SoundCanvas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ApplicationDb db;

        public ArtistController(ApplicationDb db)
        {
            this.db = db;
        }

        [HttpGet("get-all")]
        public ActionResult<List<ArtistDto>> GetAll()
        {
            var artists = db.Artists
                .Select(x => new ArtistDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PhotoUrl = x.PhotoUrl,
                    Genre = x.Genre.Name
                })
                .ToList();
            return artists;
        }

        [HttpGet("get-one/{id}")]
        public ActionResult<ArtistDto> GetOne(int id)
        {
            var artist = db.Artists
                .Where(x => x.Id == id)
                .Select(x => new ArtistDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PhotoUrl = x.PhotoUrl,
                    Genre = x.Genre.Name
                }).FirstOrDefault();

            if (artist == null)
            {
                return NotFound();
            }
            return artist;
        }

        [HttpPost("create")]
        public IActionResult Create(ArtistDto request)
        {
            if (IsArtistExits(request.Name))
            {
                return BadRequest("Artist name already exists..");
            }
            var genre = GetGenreByName(request.Genre);
            if (genre == null)
            {
                return BadRequest("Invalid Genre");
            }
            var newArtist = new Artist
            {
                Name = request.Name.ToLower(),
                PhotoUrl = request.PhotoUrl,
                Genre = genre
            };
            db.Artists.Add(newArtist);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetOne), new { id = newArtist.Id }, null);
            //return NoContent();
        }

        [HttpPut("update")]
        public IActionResult Update(ArtistAddEditDto request)
        {
            var fetchedArtist = db.Artists.Find(request.Id);

            if (fetchedArtist == null)
            {
                return NotFound();
            }

            if (fetchedArtist.Name.ToLower() != request.Name.ToLower() && IsArtistExits(request.Name))
            {
                return BadRequest("Artist name should be quinue");
            }

            var fetchedGenre = GetGenreByName(request.Genre);
            if (fetchedGenre == null)
            {
                return BadRequest("Invalid Genre");
            }
            // updating artist record
            fetchedArtist.Name = request.Name.ToLower();
            fetchedArtist.PhotoUrl = request.PhotoUrl.ToLower();
            fetchedArtist.Genre = fetchedGenre;
            db.SaveChanges();
            return NoContent();

        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }
            db.Artists.Remove(artist);
            db.SaveChanges();
            return NoContent();

        }
        private bool IsArtistExits(string name)
        {
            return db.Artists.Any(x => x.Name.ToLower() == name.ToLower());
        }

        
        private Genre GetGenreByName(string name)
        {
            return db.Genres.SingleOrDefault(x => x.Name.ToLower() == name.ToLower());
        }

    }
}
