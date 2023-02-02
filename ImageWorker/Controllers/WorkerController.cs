using ImageMagick;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageWorker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        // GET api/<WorkerController>/5
        [HttpGet()]
        public IActionResult Get([FromForm] IFormFile imageFile)
        {
            using (var stream = new MemoryStream())
            {
                imageFile.CopyTo(stream);

                stream.Position = 0;

                // Read image that needs a watermark
                using (var image = new MagickImage(stream))
                {
                    //var asd = Directory.GetFiles("Images/watermark.webp").ToString();
                    var dir = Directory.GetFileSystemEntries("/app/Images");
                    // Read the watermark that will be put on top of the image
                    using (var watermark = new MagickImage(dir[0]))
                    {
                        // Draw the watermark in the bottom right corner
                        image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);

                        // Optionally make the watermark more transparent
                        watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);

                        // Or draw the watermark at a specific location
                        image.Composite(watermark, 200, 50, CompositeOperator.Over);
                    }

                    using (var stream2 = new MemoryStream()) {
                        image.Write(stream2);
                        Byte[] bytes = stream2.ToArray();
                        return File(bytes, "image/jpeg");
                    }
                }


            }
        }

        // POST api/<WorkerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<WorkerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WorkerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
