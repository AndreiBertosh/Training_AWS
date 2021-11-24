using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonWebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;

namespace AmazonWebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RDSController : ControllerBase
    {
        private readonly IRDSService _rDSService;
        private readonly IS3Service _S3Service;
        private readonly ISQSService _SQSService;


        public RDSController(IRDSService rDSService, IS3Service s3Service, ISQSService sQSService)
        {
            _rDSService = rDSService;
            _S3Service = s3Service;
            _SQSService = sQSService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var url = HttpContext.Request.GetEncodedUrl();

            await _rDSService.Add(file);
            await _S3Service.UploadImage(file);
            await _SQSService.PushMessage(file, url);

            return Ok();
        }

        [HttpGet("File")]
        public async Task<IActionResult> GetMetadata(string name)
        {
            var image = await _rDSService.Get(name);

            return Ok(image);
        }

        [HttpGet("File/random")]
        public async Task<IActionResult> GetRandomMetadata()
        {
            var file = await _rDSService.GetRandomFile();

            return Ok(file);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(string name)
        {
            await _rDSService.Delete(name);

            return NoContent();
        }
    }
}
