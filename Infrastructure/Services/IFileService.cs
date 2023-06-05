using  Microsoft.AspNetCore.Http;
public interface IFileService
{
    Task<string> CreateFileAsync(string folder,IFormFile file);
    bool DeleteFile(string folder, string filename);
}
