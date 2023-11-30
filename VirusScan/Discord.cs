using Discord;
using Discord.Webhook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace VirusScan
{
    internal class Discord
    {
     
        public static async Task SendDiscordResults(string Webhook, string scanId, string Positives, string Resource, string md5, string SHA1, string SHA256, string PermaLink, string fileName)
        {
            var Dwbhook = new DiscordWebhookClient(Webhook);

            var eb = new EmbedBuilder();
            eb.Title = "Scan result";
            eb.AddField("File name", fileName, inline: true);
            eb.AddField("Positives", Positives, inline: true);
            eb.AddField("Hash MD5", md5, inline: true);
            eb.AddField("Hash SHA1", SHA1, inline: true);
            eb.AddField("Hash SHA256", SHA256, inline: true);
            eb.AddField("Virus total Link", PermaLink, inline: true);
            eb.AddField("Resource", Resource, inline: true);
            Embed[] embedArray = new Embed[] { eb.Build() };

            await Dwbhook.SendFileAsync(filePath: Directory.GetCurrentDirectory() + "\\image.png", text: null, embeds: embedArray);

        }
    }

    }

