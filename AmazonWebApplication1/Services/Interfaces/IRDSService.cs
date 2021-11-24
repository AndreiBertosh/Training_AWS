using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonWebApplication1.Models;
using Amazon.S3.Model;

namespace AmazonWebApplication1.Services.Interfaces
{
    public interface IRDSService
    {
        Task Add(IFormFile file);

        Task Delete(string name);

        Task<ImageModel> Get(string name);

        Task<ImageModel> GetRandomFile();

        //Task<(byte[] Body, string ContentType)> GetImage(string name);

        //Task<DeleteObjectResponse> DeleteImage(string name);

        //Task<PutObjectResponse> UploadImage(IFormFile file);
    }
}
