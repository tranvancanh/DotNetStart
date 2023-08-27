namespace WebApi.Commons
{
    public class FileTextHandle
    {

        public static void CreateNewFile(string textFilePath)
        {
            // ファイルが存在しない場合は、ファイルを作成します
            if (!File.Exists(textFilePath))
            {
                using (StreamWriter sw = File.CreateText(textFilePath))
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
            return;
        }

        public static void DeleteFile(string textFilePath)
        {
            // ファイルが存在しない場合は、ファイルを作成します
            if (!File.Exists(textFilePath))
            {
                File.Delete(textFilePath);
            }
            return;
        }

        public static void OpenBlankFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(fileName, FileMode.Truncate))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return;
        }
    }
}
