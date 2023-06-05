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
    public async Task<List<GetQuoteDto>> GetQuotesAsync()
    {
        return await _quoteService.GetQuotesAsync();
    }

    [HttpPost("Add")]
    public async Task<GetQuoteDto> AddQuoteAsync([FromForm] AddQuoteDto quote)
    {
        return await _quoteService.AddQuoteAsync(quote);
    }

    [HttpPut("Update")]
    public async Task<GetQuoteDto> UpdateQuoteAsync([FromForm] AddQuoteDto quote)
    {
        return await _quoteService.UpdateQuoteAsync(quote);
    }
}
