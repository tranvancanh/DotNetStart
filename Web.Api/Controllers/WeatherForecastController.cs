using ChinhDo.Transactions.FileManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Transactions;
using WebApi.Models;

namespace Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("api/download")]
        public IActionResult DownloadFile1()
        {
            // Replace "path/to/your/file.pdf" with the actual path of the file you want to download.
            string filePath = "path/to/your/file.pdf";

            try
            {
                // Determine the content type (MIME type) of the file based on its extension.
                string contentType = "application/octet-stream"; // Binary data, change according to your file type.
                if (Path.GetExtension(filePath).ToLowerInvariant() == ".pdf")
                {
                    contentType = "application/pdf";
                }
                else if (Path.GetExtension(filePath).ToLowerInvariant() == ".txt")
                {
                    contentType = "text/plain";
                }
                // Add more conditions for other file types if needed.

                // Open the file as a stream.
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Return the stream using the PhysicalFile method with appropriate content type and file name.
                return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur.
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("api/download")]
        public IActionResult DownloadFile2()
        {
            // Replace "path/to/your/file.pdf" with the actual path of the file you want to download.
            string filePath = "path/to/your/file.pdf";

            try
            {
                // Create a FileStream to open the file for reading.
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Determine the content type (MIME type) of the file based on its extension.
                string contentType = "application/octet-stream"; // Binary data, change according to your file type.
                if (Path.GetExtension(filePath).ToLowerInvariant() == ".pdf")
                {
                    contentType = "application/pdf";
                }
                else if (Path.GetExtension(filePath).ToLowerInvariant() == ".txt")
                {
                    contentType = "text/plain";
                }
                // Add more conditions for other file types if needed.

                // Return the FileStream as a FileStreamResult.
                return new FileStreamResult(fileStream, contentType)
                {
                    FileDownloadName = Path.GetFileName(filePath) // Set the desired file name for download.
                };
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur.
                return BadRequest("Error: " + ex.Message);
            }
        }

        private string GetNextFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string pathName = Path.GetDirectoryName(fileName);
            string fileNameOnly = Path.Combine(pathName, Path.GetFileNameWithoutExtension(fileName));
            int i = 0;
            // If the file exists, keep trying until it doesn't
            while (System.IO.File.Exists(fileName))
            {
                i += 1;
                fileName = string.Format("{0}({1}){2}", fileNameOnly, i, extension);
            }
            return fileName;
        }

        private void TransationMoveFile()
        {
            var srcPath = "\\\\nas1\\d\\data\\email\\KUNO\\test";
            var destPath = "\\\\nas1\\d\\data\\email\\KUNO\\test\\bak";
            string[] filePaths = Directory.GetFiles(srcPath);
            IFileManager fm = new TxFileManager();
            using (TransactionScope scope1 = new TransactionScope())
            {
                foreach(var file in filePaths)
                {
                    var fileName = Path.GetFileName(file);
                    var fullFile = Path.Combine(destPath, fileName);
                    // Move a file
                    fm.Move(file, fullFile);
                }

                scope1.Complete();
            }
        }

        private void Test1()
        {
            dynamic d1 = 100;
            int i = d1;

            d1 = "Hello";
            string greet = d1;

            d1 = DateTime.Now;
            DateTime dt = d1;
        }

    }
}