using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonWebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AmazonWebApplication1.Services
{
    public class S3Service : IS3Service
    {
        private const string _bucketName = "andrei-bertosh-application-bucket";
        private readonly IAmazonS3 _amazonS3Client;

        public S3Service(IAmazonS3 amazonS3Client)
        {
            _amazonS3Client = amazonS3Client;
        }

        public async Task<DeleteObjectResponse> DeleteImage(string name)
        {
            return await _amazonS3Client.DeleteObjectAsync(_bucketName, name);
        }

        public async Task<(byte[] Body, string ContentType)> GetImage(string name)
        {
            var response = await _amazonS3Client.GetObjectAsync(_bucketName, name);
            using var stream = response.ResponseStream;
            var buffer = new byte[response.ContentLength];
            await stream.ReadAsync(buffer);

            return (buffer, response.Headers.ContentType);
        }

        public async Task<PutObjectResponse> UploadImage(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                ContentType = file.ContentType,
                Key = file.FileName,
                InputStream = stream,
            };
            var response = await _amazonS3Client.PutObjectAsync(putRequest);

            return response;


        }
    }
}
