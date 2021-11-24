using System.Collections.Generic;
using System.Threading.Tasks;
using AmazonWebApplication1.Models;

namespace AmazonWebApplication1.Services.Interfaces
{
    public interface IMessageService
    {
        Task ProcessQueue();

        Task<int> GetMessagesCount();

        Task<List<string>> ExtractMessages();
    }
}
