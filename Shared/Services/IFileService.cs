
namespace Shared.Services
{
    public interface IFileService
    {
        void CreateFile(string fileName);
        void DeleteFile(string fileName);
        void WriteToFile(string fileName, string content);
        string ReadFile(string fileName);
    }
}
