using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using AmazonWebApplication1.Models;
using AmazonWebApplication1.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AmazonWebApplication1.Services
{
    public class MessageService : IMessageService
    {
        private const string TopicARN = "arn:aws:sns:us-west-2:617047695299:Task8-uploads-notification-topic";
        private string _queueUrl = "https://sqs.us-west-2.amazonaws.com/617047695299/Task8-uploads-notification-queue";

        private readonly IAmazonSimpleNotificationService _SNSService;
        private readonly IAmazonSQS _SQSService;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IAmazonSQS sQSService, IAmazonSimpleNotificationService sNSService, ILogger<MessageService> logger)
        {
            _SNSService = sNSService;
            _SQSService = sQSService;
            _logger = logger;
        }

        public async Task ProcessQueue()
        {
            var response = await _SQSService.ReceiveMessageAsync(_queueUrl);
            var messages = response.Messages;

            _logger.LogInformation($"{messages.Count} messages was received.");

            foreach (var message in messages)
            {
                await _SNSService.PublishAsync(TopicARN, message.Body);
                await _SQSService.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
            }
        }

        public async Task<int> GetMessagesCount()
        {
            var response = await _SQSService.ReceiveMessageAsync(_queueUrl);
            var message = response.Messages;

            _logger.LogInformation($"{message.Count} messages was received.");

            return message.Count;
        }

        public async Task<List<string>> ExtractMessages()
        {
            var response = await _SQSService.ReceiveMessageAsync(_queueUrl);
            var messages = response.Messages;
            _logger.LogInformation($"{messages.Count} messages was received.");

            var result = messages.Select(m => m.Body).ToList();

            return result;
        }
    }
}
