using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AmazonWebApplication1.Services.Interfaces;

namespace AmazonWebApplication1.Services
{
    public class SNSService : ISNSService
    {
        private const string TopicARN = "arn:aws:sns:us-west-2:617047695299:Task8-uploads-notification-topic";
        private readonly IAmazonSimpleNotificationService _notificationService;

        public SNSService(IAmazonSimpleNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<SubscribeResponse> Subscribe(string email)
        {
            var subscribeRquest = new SubscribeRequest
            {
                TopicArn = TopicARN,
                Endpoint = email,
                Protocol = "email"
            };

            var responce = await _notificationService.SubscribeAsync(subscribeRquest);

            return responce;
        }

        public async Task<UnsubscribeResponse> Unsubscribe(string email)
        {
            var subscriptions = await _notificationService.ListSubscriptionsByTopicAsync(TopicARN);

            if (!subscriptions.Subscriptions.Any())
            {
                return new UnsubscribeResponse
                {
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var subscription = subscriptions.Subscriptions.FirstOrDefault(s => s.Endpoint == email);

            var request = new UnsubscribeRequest
            {
                SubscriptionArn = subscription.SubscriptionArn
            };

            var responce = await _notificationService.UnsubscribeAsync(request);

            return responce;
        }
    }
}
