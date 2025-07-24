using System;
using System.IO;

namespace Shared.Services
{
    internal class FileService : IFileService
    {
        public void CreateFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            if (File.Exists(fileName))
                throw new IOException($"File '{fileName}' already exists.");

            using (File.Create(fileName)) { }
        }

        public void DeleteFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"File '{fileName}' does not exist.", fileName);

            File.Delete(fileName);
        }

        public string ReadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"File '{fileName}' does not exist.", fileName);

            return File.ReadAllText(fileName);
        }

        public void WriteToFile(string fileName, string content)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            File.WriteAllText(fileName, content ?? string.Empty);
        }
    }
}
