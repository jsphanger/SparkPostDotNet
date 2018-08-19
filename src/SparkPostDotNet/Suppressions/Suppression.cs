using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SparkPostDotNet.Suppressions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Suppression : HttpContent
    {
        [JsonProperty("recipient")]
        public string RecipientEmail { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var content = this.GetContent();

            await stream.WriteAsync(content, 0, content.Length);
        }
        protected override bool TryComputeLength(out long length)
        {
            length = this.GetContent().Length;

            return true;
        }
        protected byte[] GetContent()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}
