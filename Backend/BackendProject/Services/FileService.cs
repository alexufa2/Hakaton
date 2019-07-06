using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.Services
{
    public interface IFileService
    {
        string SaveFileByName(string fileName, string fileContent);

        string SaveFileByPath(string filePath, string fileContent);

        string ReadFileByName(string fileName);

        string ReadFileByPath(string filePath);
    }


    public class FileService : IFileService
    {
        private string _fileDirPath;


        public FileService(string fileDirPath)
        {
            _fileDirPath = fileDirPath;
        }


        public string ReadFileByName(string fileName)
        {
            throw new NotImplementedException();
        }

        public string ReadFileByPath(string fileDir,)
        {
            throw new NotImplementedException();
        }

        public string SaveFileByName(string fileName, string fileContent)
        {
            string filePath = Path.Combine(_fileDirPath, fileName);
            return SaveFileByPath(filePath, fileContent);
        }

        public string SaveFileByPath(string filePath, string fileContent)
        {
            string dir= direc
           Directory.Exists()
        }
    }
}
