using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonWebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.IO;

namespace AmazonWebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SQSController : ControllerBase
    {
        private readonly ISQSService _sQSService;

        public SQSController(ISQSService sQSService)
        {
            _sQSService = sQSService;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var url = HttpContext.Request.GetEncodedUrl();

            await _sQSService.PushMessage(file, url);

            return Ok();
        }

        [HttpPost("UploadTxtFile")]
        public async Task<IActionResult> UploadTxtFile(IFormFile file)
        {
            await _sQSService.GetFileAndPush(file);

            return Ok();
        }

        [HttpPost("DownloadTxtFile")]
        public async Task<IActionResult> DownloadTxtFile()
        {
            await _sQSService.ReadAllMessage();

            return Ok();
        }

    }
}
