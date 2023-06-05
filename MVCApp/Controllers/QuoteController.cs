using Domain.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace MVCApp.Controllers;

public class QuoteController : Controller
{
    private readonly QuoteService _quoteService;
    public QuoteController(QuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    public async Task<IActionResult> Index()
    {
        var quotes = await _quoteService.GetQuotesAsync(); 
        return View(quotes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AddQuoteDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddQuoteDto quote)  
    {
        if(ModelState.IsValid)
        {
            await _quoteService.AddQuoteAsync(quote);
            return RedirectToAction("Index");
        }
        return View(quote);
    }


        [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var existing = await _quoteService.GetQuotesByIdAsync(id);
        var addQuote = new AddQuoteDto()
        {
            Id = existing.Id,
            Author = existing.Author,
            CategoryId = existing.CategoryId,
            QuoteText = existing.QuoteText

        };
        return View(addQuote);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AddQuoteDto quote)  
    {
        if(ModelState.IsValid)
        {
            await _quoteService.UpdateQuoteAsync(quote);
            return RedirectToAction("Index");
        }
        return View(quote);
    }


    [HttpGet]
    public  IActionResult DeleteQuote(int id)  
    {
        _quoteService.DeleteQuote(id);
        return RedirectToAction("Index");  
    }
}
