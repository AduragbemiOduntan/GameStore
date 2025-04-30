using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{
    public record class UpdateGameDto(
        [Required(ErrorMessage ="Name is required")][StringLength(50)]
        string Name,
        [Required][StringLength(20)]
       int GenreId,
        [Range(1, 100)]
        decimal Price,
        DateOnly ReleaseDate);

}
