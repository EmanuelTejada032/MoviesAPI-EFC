namespace MoviesAPI_EFC.Services.Contract
{
    public interface IFileManager
    {
        Task<string> UploadFile(byte[] content, string extension , string container, string contentType);
        Task<string> EditFile(byte[] content, string path, string extension, string container, string contentType);
        Task DeleteFile(string path, string container);
    }
}
