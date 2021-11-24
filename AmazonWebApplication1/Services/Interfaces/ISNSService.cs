using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;

namespace AmazonWebApplication1.Services.Interfaces
{
    public interface ISNSService
    {
        Task<SubscribeResponse> Subscribe(string email);

        Task<UnsubscribeResponse> Unsubscribe(string email);
    }
}
