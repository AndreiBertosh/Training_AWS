using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using AmazonWebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using AmazonWebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace AmazonWebApplication1.Services
{
    public class SQSService : ISQSService
    {

        private string _queueUrl = "https://sqs.us-west-2.amazonaws.com/617047695299/Task8-uploads-notification-queue";
        private readonly IAmazonSQS _amazonSQSClient;
        private int chunkCharacters = 1000;

        public SQSService(IAmazonSQS amazonSQSClient)
        {
            _amazonSQSClient = amazonSQSClient;
        }

        public async Task PushMessage(IFormFile file, string url)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                MessageBody = $"New file added {file.FileName} {DateTime.UtcNow}, file size is {file.Length}, Download link {url}/{file.FileName}",
                QueueUrl = _queueUrl,
            };

            await _amazonSQSClient.SendMessageAsync(sendMessageRequest);
        }

        public async Task GetFileAndPush(IFormFile file)
        {
            try
            {
                var reader = new StreamReader(file.OpenReadStream());
                var lines = await reader.ReadToEndAsync();

                var messagesBody = new List<SQSMessageModel>();

                var count = 0;
                var blockCount = 1;
                var stopReadLines = false;

                while (!stopReadLines)
                {
                    var body = string.Empty;
                    if (lines.Length - (chunkCharacters * count) > chunkCharacters)
                    {
                        body = lines.Substring(chunkCharacters * count, chunkCharacters);
                    }
                    else
                    {
                        body = lines.Substring(chunkCharacters * count);
                    }
                    messagesBody.Add(new SQSMessageModel
                    {
                        Id = blockCount,
                        FileName = file.FileName,
                        FileLines = body
                    });

                    blockCount++;
                    count++;
                    if (chunkCharacters * count > lines.Length)
                    {
                        stopReadLines = true;
                    }
                }

                var blocksCount = messagesBody.Count;
                messagesBody.ForEach(r => r.BlocksCount = blocksCount);

                foreach (var message in messagesBody)
                {
                    await _amazonSQSClient.SendMessageAsync(_queueUrl, ToJson(message, false).ToString());
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }
        }

        public async Task ReadAllMessage()
        {
            var request = new ReceiveMessageRequest() 
            {
                AttributeNames = new List<string>() { "All" },
                MaxNumberOfMessages = 10,
                QueueUrl = _queueUrl
            };

            var stopReciveMessages = false;

            var fileChunks = new List<SQSMessageModel>();

            while (!stopReciveMessages)
            { 
                var response = await _amazonSQSClient.ReceiveMessageAsync(_queueUrl);
                var messages = response.Messages;

                if (messages.Count == 0)
                {
                    stopReciveMessages = true;
                }

                foreach (var message in messages)
                {
                    fileChunks.Add(JsonConvert.DeserializeObject<SQSMessageModel>(message.Body));
                    await _amazonSQSClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                }
            }

            if (fileChunks.Any())
            {
                var lines = fileChunks.OrderBy(i => i.Id).Select(l => l.FileLines);
                var fileName = fileChunks.FirstOrDefault().FileName;

                await File.WriteAllLinesAsync($"D:\\EPAM\\Learn\\.NET Intermediate Mentoring Program #1\\Task3\\Result\\{fileName}", lines.ToArray());
            }
        }

        public string ToJson(object o, bool format = true)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            var formatting = format ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(o, formatting, settings);
        }
    }
}
