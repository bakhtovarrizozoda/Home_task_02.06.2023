using  Microsoft.AspNetCore.Http;
public interface IFileService
{
    string CreateFile(string folder,IFormFile file);
    bool DeleteFile(string folder, string filename);
}
