
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class AddQuoteDto : QuoteDto
{
    public IFormFile File { get; set; }
}
