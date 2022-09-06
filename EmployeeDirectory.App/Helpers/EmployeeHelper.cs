using NUlid;

using System.Linq;

namespace EmployeeDirectory.App.Helpers
{
    internal static class EmployeeHelper
    {
        public static string GenerateEmployeeRowPhotoLink(string fileName)
        {
            var fileExtension = GetFileExtension(fileName);

            if (IsValidExtension(
                fileExtension, new string[] { "jpg", "jpeg", "png", "tiff" }))
            {
                return GenerateRandomPath(fileExtension);
            }

            return "/EmployeePhoto/_defaultUser.jpg";
        }

        private static string GenerateRandomPath(string fileExtension)
        {
            return "/EmployeePhoto/Data/" + Ulid.NewUlid().ToString() + '.' + fileExtension;
        }

        private static string GetFileExtension(string fileName)
        {
            return fileName.Split('.')[^1];
        }

        private static bool IsValidExtension(string checkExtension, string[] extensions)
        {
            return extensions.Any(value => value == checkExtension.ToLower());
        }
    }
}
