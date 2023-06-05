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

    public async Task<List<GetQuoteDto>> GetQuotesAsync()
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = $"select id as Id, author as Author,quote_text  as QuoteText,category_id as CategoryId, file_name as FileName from quotes order by id";
            var result = await conn.QueryAsync<GetQuoteDto>(sql);
            return result.ToList();
        }
    }

    public async Task<GetQuoteDto> GetQuotesByIdAsync(int id)
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = $"select id as Id, author as Author,quote_text  as QuoteText,category_id as CategoryId, file_name as FileName from quotes where id = @id";
            var result = await conn.QuerySingleOrDefaultAsync<GetQuoteDto>(sql, new {id});
            return result;
        }
    }

    public async Task<GetQuoteDto> AddQuoteAsync(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var filename = await _fileService.CreateFileAsync("images", quote.File);
            var sql = $"insert into quotes (author, quote_text, category_id, file_name) VALUES (@Author, @QuoteText, @CategoryId, @filename) returning id;";
            var result = await conn.ExecuteScalarAsync<int>(sql, new
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

    public async Task<GetQuoteDto> UpdateQuoteAsync(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var sql1 = $"select id , author ,quote_text ,category_id ,file_name  from quotes where id=@id;";
            var existing =  conn.QuerySingleOrDefault<GetQuoteDto>(sql1, new { quote.Id });
            if (existing == null)
            {
                return new GetQuoteDto();
            }

            string filename = null;

            if (quote.File != null && existing.FileName != null)
            {
                _fileService.DeleteFile("images", existing.FileName);
                filename = await _fileService.CreateFileAsync("images", quote.File);
            }
            else if (quote.File != null && existing.FileName == null)
            {
                filename = await _fileService.CreateFileAsync("images", quote.File);
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

    public void DeleteQuote(int id)
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = $"delete from Quotes WHERE id = {id}";
            var result = conn.Execute(sql); 
        }
    } 
}
