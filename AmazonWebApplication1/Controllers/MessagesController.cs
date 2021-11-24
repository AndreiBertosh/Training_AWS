using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonWebApplication1.Services.Interfaces;

namespace AmazonWebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messagesService;

        public MessagesController(IMessageService messagesService)
        {
            _messagesService = messagesService;
        }

        [HttpGet("messages count")]
        public async Task<IActionResult> GetSQSQueueMesssagesCount()
        {
            var messagesCount = await _messagesService.GetMessagesCount();

            return Ok(messagesCount);
        }

        [HttpGet("message bodies")]
        public async Task<IActionResult> GetSQSQueueMesssagesBodies()
        {
            var messagesCount = await _messagesService.ExtractMessages();

            return Ok(messagesCount);
        }

        [HttpPost("publish messages to SNS")]
        public async Task<IActionResult> PublishMessagesToSNS()
        {
            await _messagesService.ProcessQueue();

            return Ok();
        }

    }
}
