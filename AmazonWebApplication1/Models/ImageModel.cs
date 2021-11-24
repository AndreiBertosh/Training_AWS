using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonWebApplication1.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string FileExtension { get; set; }
    }
}
