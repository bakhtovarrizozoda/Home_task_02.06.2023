using Dapper;
using Domain.Dtos;
using Infrastructure.Context;

namespace Infrastructure.Services;

public class QuoteService
{
    private readonly DapperContext _context;
    private readonly IFileService _fileService;

    public QuoteService(DapperContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public List<GetQuoteDto> GetQuotes()
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = $"select id as Id, author as Author,quote_text  as QuoteText,category_id as CategoryId, file_name as FileName from quotes";
            var result = conn.Query<GetQuoteDto>(sql).ToList();
            return result.ToList();
        }
    }

    public GetQuoteDto AddQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var filename = _fileService.CreateFile("images", quote.File);
            var sql = $"insert into quotes (author, quote_text, category_id, file_name) VALUES (@Author, @QuoteText, @CategoryId, @filename) returning id;";
            var result = conn.ExecuteScalar<int>(sql, new
            {
                quote.Author,
                quote.QuoteText,
                quote.CategoryId,
                filename
            });
            return new GetQuoteDto()
            {
                Author = quote.Author,
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename,
                Id = result
            };
        }
    }

    public GetQuoteDto UpdateQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var existing = conn.QuerySingleOrDefault<GetQuoteDto>("select id , author ,quote_text ,category_id ,file_name  from quotes where id=@id;",
                    new { quote.Id });
            if (existing == null)
            {
                return new GetQuoteDto();
            }

            string filename = null;

            if (quote.File != null && existing.FileName != null)
            {
                _fileService.DeleteFile("images", existing.FileName);
                filename = _fileService.CreateFile("images", quote.File);
            }
            else if (quote.File != null && existing.FileName == null)
            {
                filename = _fileService.CreateFile("images", quote.File);
            }
            var sql = $"update quotes set author=@Author, quote_text=@QuoteText,category_id=@CategoryId  where id=@Id";
            if (quote.File != null)
            {
                sql = $"update quotes set author=@author, quote_text=@QuoteText,category_id=@CategoryId,file_name=@FileName where id=@id";

            }
            var result = conn.Execute(sql, new
            {
                quote.Author,
                quote.QuoteText,
                quote.CategoryId,
                filename,
                quote.Id
            });
            return new GetQuoteDto()
            {
                Author = quote.Author,
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename,
                Id = result
            };
        }
    }
}
