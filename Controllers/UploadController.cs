using System.IO;
using System.Net.Http.Headers;
using ImageRecognitionServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageRecognitionServer.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller 
    {
        [HttpPost]
        public IActionResult UploadImage() 
        {
            const string Action = "Uploading images";

            var files = Request.Form.Files;

            foreach (var file in files)
            {
                if (file == null) 
                {
                    return Json(new ResponseEntity(){
                        Action = Action,
                        Error = "Image doesn't exist",
                        ImageResponse = null,
                        Status = "Failed"
                    });
                } 
                else if (file.Length == 0) 
                {
                    return Json(new ResponseEntity(){
                        Action = "Upload image",
                        Error = "Image has broken data",
                        ImageResponse = null,
                        Status = "Failed"
                    });
                }
                string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img");
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di =
                        Directory.CreateDirectory(path);
                }
                using (var readStream = file.OpenReadStream())
                {
                    var filename = ContentDispositionHeaderValue
                                            .Parse(file.ContentDisposition)
                                            .FileName
                                            .Trim('"');
                    filename = path + $@"\{filename}";

                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            return Json(new ResponseEntity() {
                    Action = Action,
                    Error = null,
                    ImageResponse = null,
                    Status = "Success"
                });
        }
    }
}