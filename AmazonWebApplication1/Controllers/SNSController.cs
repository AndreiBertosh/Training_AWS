using System.Net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonWebApplication1.Services.Interfaces;
using Amazon.S3;

namespace AmazonWebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SNSController : ControllerBase
    {
        private readonly ISNSService _sNSService;
        IAmazonS3 S3Client { get; set; }

        public SNSController(ISNSService sNSService, IAmazonS3 s3Client)
        {
            _sNSService = sNSService;
            this.S3Client = s3Client;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(string email)
        {
            var response = await _sNSService.Subscribe(email);

            return response.HttpStatusCode is HttpStatusCode.OK
         ? NoContent()
         : StatusCode((int)response.HttpStatusCode);
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe(string email)
        {
            var response = await _sNSService.Unsubscribe(email);

            return response.HttpStatusCode is HttpStatusCode.OK
               ? NoContent()
               : StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
