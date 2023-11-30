using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusScan
{
    internal class Settings
    {

        public class Whook
        {
            public string Webhook { get; set; }

        }

        public static readonly string VirusTotalApiKey = "3cb087de8251e2f145c12aab0e328460786c7ccc67403e03a50d5ef27d85a5f9";
        public static readonly string Webhook = ReadWebhookConfig();

        public static string ReadWebhookConfig()
        {
            if (File.Exists("config.json"))
            {
                var text = File.ReadAllText("config.json").Trim();
                var content = JsonConvert.DeserializeObject<Whook>(text);
                return content.Webhook.ToString();
            }

            return "";
        }
    }
}
