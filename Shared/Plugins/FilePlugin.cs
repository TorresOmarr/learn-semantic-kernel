using Microsoft.SemanticKernel;
using Shared.Services;
using System.ComponentModel;

namespace Shared.Plugins
{
    public class FilePlugin
    {
        private readonly IFileService _fileService;
        public FilePlugin(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        [KernelFunction("create_file")]
        [Description("Creates a new file with the specified name.")]
        public void CreateFile([Description("Name of the file to create")] string fileName)
        {
            _fileService.CreateFile(fileName);
        }
        [KernelFunction("delete_file")]
        [Description("Deletes the specified file.")]
        public void DeleteFile([Description("Name of the file to delete")] string fileName)
        {
            _fileService.DeleteFile(fileName);
        }
        [KernelFunction("write_to_file")]
        [Description("Writes content to the specified file.")]
        public void WriteToFile([Description("Name of the file to write to")] string fileName, 
                                [Description("Content to write to the file")] string content)
        {
            _fileService.WriteToFile(fileName, content);
        }
        [KernelFunction("read_file")]
        [Description("Reads content from the specified file.")]
        public string ReadFile([Description("Name of the file to read from")] string fileName)
        {
            return _fileService.ReadFile(fileName);
        }
    }
}
