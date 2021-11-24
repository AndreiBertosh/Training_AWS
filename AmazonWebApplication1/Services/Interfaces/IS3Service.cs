using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AmazonWebApplication1.Services.Interfaces
{
    public interface IS3Service
    {
        Task<DeleteObjectResponse> DeleteImage(string name);

        Task<(byte[] Body, string ContentType)> GetImage(string name);

        Task<PutObjectResponse> UploadImage(IFormFile file);

    }
}
