using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AmazonWebApplication1.Database;
using AmazonWebApplication1.Models;
using AmazonWebApplication1.Services.Interfaces;

namespace AmazonWebApplication1.Services
{
    public class RDSService : IRDSService
    {
        private readonly RDSContext _rDSContext;

        public RDSService(RDSContext rDSContext)
        {
            _rDSContext = rDSContext;
        }

        public async Task Add(IFormFile file)
        {
            _rDSContext.ImageModel.Add(new ImageModel
            {
                UpdatedAt = DateTime.UtcNow,
                FileExtension = file.ContentType,
                Name = file.FileName,
                Size = file.Length
            });

            await _rDSContext.SaveChangesAsync();
        }

        public async Task Delete(string name)
        {
            var metadataToDelete = await _rDSContext.ImageModel.FirstOrDefaultAsync(x => x.Name == name);
            _rDSContext.ImageModel.Remove(metadataToDelete);
            await _rDSContext.SaveChangesAsync();

        }

        public async Task<ImageModel> Get(string name)
        {
            return await _rDSContext.ImageModel.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<ImageModel> GetRandomFile()
        {
            var models = await _rDSContext.ImageModel.ToListAsync();
            if (models.Count > 0)
            {
                var index = new Random((int)DateTime.UtcNow.Ticks).Next(0, models.Count);
                return models[index];
            }

            return null;
        }

    }
}
