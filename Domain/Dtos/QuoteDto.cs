using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class QuoteDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please fill the Author")]
    public string Author { get; set; }
    [Required(ErrorMessage = "Quote Text is required")]
    public string QuoteText { get; set; }
    [Required(ErrorMessage = "Category id is required")]
    public int CategoryId { get; set; }
}
