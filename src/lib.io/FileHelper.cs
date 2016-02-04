using System.IO;

namespace lib
{
    public static class FileHelper
    {
        public static bool DeleteIfExists(string filePath)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(filePath)
                   && File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return result;
        }
    }
}
