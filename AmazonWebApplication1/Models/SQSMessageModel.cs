using Newtonsoft.Json;

namespace AmazonWebApplication1.Models
{
    public class SQSMessageModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("blockCount")]
        public int BlocksCount { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("lines")]
        public string FileLines { get; set; }
    }
}
