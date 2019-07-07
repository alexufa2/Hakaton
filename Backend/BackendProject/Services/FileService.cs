using System.IO;

namespace TemplateApp.Services
{
    public interface IFileService
    {
        string SaveFileByName(string fileName, string fileContent);

        string SaveFileByPath(string fileDir, string fileName, string fileContent);

        string ReadFileByName(string fileName);

        string ReadFileByPath(string fileDir, string fileName);
    }


    public class FileService : IFileService
    {
        private string _fullFileDirPath;


        public FileService(string appRoot, string fileDirPath)
        {
            _fullFileDirPath = Path.Combine(appRoot, fileDirPath);
        }


        public string ReadFileByName(string fileName)
        {
            return ReadFileByPath(_fullFileDirPath, fileName);
        }

        public string ReadFileByPath(string fileDir, string fileName)
        {
            string path = Path.Combine(fileDir, fileName);
            if (!File.Exists(path))
            {
                return string.Empty;
            }

            return File.ReadAllText(path);
        }

        public string SaveFileByName(string fileName, string fileContent)
        {
            return SaveFileByPath(_fullFileDirPath, fileName, fileContent);
        }

        public string SaveFileByPath(string fileDir, string fileName, string fileContent)
        {
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            string path = Path.Combine(fileDir, fileName);
            File.WriteAllText(path, fileContent);
            return path;
        }
    }
}
