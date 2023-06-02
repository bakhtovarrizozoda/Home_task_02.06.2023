using Domain.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileUploadController
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly QuoteService _quoteService;

    public FileUploadController(IWebHostEnvironment webHostEnvironment, QuoteService quoteService)
    {
        _webHostEnvironment = webHostEnvironment;
        _quoteService = quoteService;
    }
    
    [HttpGet("GetList")]
    public List<GetQuoteDto> GetQuotes()
    {
        return _quoteService.GetQuotes();
    }

    [HttpPost("Add")]
    public GetQuoteDto AddQuote([FromForm] AddQuoteDto quote)
    {
        return _quoteService.AddQuote(quote);
    }

    [HttpPut("Update")]
    public GetQuoteDto UpdateQuote([FromForm] AddQuoteDto quote)
    {
        return _quoteService.UpdateQuote(quote);
    }
}
