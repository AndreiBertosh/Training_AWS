using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AmazonWebApplication1.Services.Interfaces
{
    public interface ISQSService
    {
        Task PushMessage(IFormFile file, string url);

        Task GetFileAndPush(IFormFile file);

        Task ReadAllMessage();
    }
}
