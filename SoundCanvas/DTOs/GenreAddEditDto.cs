using System.ComponentModel.DataAnnotations;

namespace SoundCanvas.DTOs
{
    public class GenreAddEditDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
