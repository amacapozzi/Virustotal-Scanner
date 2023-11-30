using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirusTotalNet;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;
using System.Drawing;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using VirusScan.Utils;

namespace VirusScan
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


            Console.WriteLine("[+] Welcome to Virus Scanner");
            Thread.Sleep(1000);
            Console.WriteLine("[+] Developed by Student");

            var WEBHOOK_URL = "";
            Thread.Sleep(1000);
            if (!File.Exists("config.json"))
            {
                Console.WriteLine("\n[+] Please enter your webhook url to send the file results");
                string Enter = Console.ReadLine().Trim();
                WEBHOOK_URL += Enter;

                if (String.IsNullOrEmpty(WEBHOOK_URL) || String.IsNullOrWhiteSpace(WEBHOOK_URL))
                {
                    Console.WriteLine($"Webhook is invalid");
                }


                var webhookObject = new
                {
                    Webhook = WEBHOOK_URL
                };

                string json = JsonConvert.SerializeObject(webhookObject, Formatting.Indented);


                File.WriteAllText("config.json", json);
            } else
            {
                WEBHOOK_URL += Settings.Webhook;
            }
            Thread.Sleep(1000);
            Console.WriteLine("\n[+] Please enter the file path to scan");
            string filePath = Console.ReadLine().Trim();
            if (String.IsNullOrEmpty(filePath) || String.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Please enter a valid path");
                Environment.Exit(0);
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                FileInfo fileInfo = new FileInfo(filePath);
                VirusTotal virusTotal = new VirusTotal(Settings.VirusTotalApiKey);
                virusTotal.UseTLS = true;
                FileReport fileReport = await virusTotal.GetFileReportAsync(fileBytes);

                byte[] imgByte = ImageConveter.GetImageBytes(filePath);
                var base64String = Convert.ToBase64String(imgByte);

                Image img = ImageConveter.Base64ToImage(base64String);
                img.Save("image.png");


    
                bool NotChecked = false;

                    if (fileReport.ResponseCode != FileReportResponseCode.Present)
                    {
                        NotChecked = true;
                    }

                    if (NotChecked)
                    {
                        Console.WriteLine("This file wasn't uploaded, let's scan it");
                        ScanResult scanResult = await virusTotal.ScanFileAsync(filePath.ToString().Trim());
                        if (scanResult != null)
                        {
                        await Discord.SendDiscordResults(WEBHOOK_URL, fileReport.ScanId, "Wait for end scan", fileReport.Resource, fileReport.MD5, fileReport.SHA1, fileReport.SHA256, fileReport.Permalink, fileInfo.Name);
                    }

                    }
                    else
                    {
                    await Discord.SendDiscordResults(WEBHOOK_URL, fileReport.ScanId, fileReport.Positives.ToString(), fileReport.Resource, fileReport.MD5, fileReport.SHA1, fileReport.SHA256, fileReport.Permalink, fileInfo.Name);
                    File.Delete("image.png");
                    Console.WriteLine("File scanned succesfully");
                    Thread.Sleep(new Random().Next(2000, 3000));
                    Environment.Exit(0);
                    }
 
            }  catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                Console.ReadLine();
            }
          
 }
    }


}