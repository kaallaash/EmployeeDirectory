using System.IO;
using System.Threading.Tasks;

namespace EmployeeDirectory.App.Helpers
{
    public static class FileHelper
    {
        public static Task Delete(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                try
                {
                    fileInfo.Delete();
                }
                catch
                { }
            }

            return Task.CompletedTask;
        }
    }
}
