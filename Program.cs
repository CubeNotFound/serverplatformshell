using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace serverplatformshell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input.StartsWith("LOGIN", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = input.Split(char.Parse(" "));
                    var url = "http://localhost:5678/auth/login";
                    var json = $"{{\"username\":\"{split[1]}\",\"password\":\"{split[2]}\"}}";

                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    byte[] data = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseBody = reader.ReadToEnd();
                        Console.WriteLine("Status: " + response.StatusCode);
                        Console.WriteLine(responseBody);
                    }
                }
            }
        }
    }
}
