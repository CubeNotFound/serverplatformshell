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
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.Title = "CubeNotFound's Server Platform Shell";
            Console.WriteLine(" ▄████▄   █    ██  ▄▄▄▄   ▓█████  ███▄    █  ▒█████  ▄▄▄█████▓  █████▒▒█████   █    ██  ███▄    █ ▓█████▄ \r\n▒██▀ ▀█   ██  ▓██▒▓█████▄ ▓█   ▀  ██ ▀█   █ ▒██▒  ██▒▓  ██▒ ▓▒▓██   ▒▒██▒  ██▒ ██  ▓██▒ ██ ▀█   █ ▒██▀ ██▌\r\n▒▓█    ▄ ▓██  ▒██░▒██▒ ▄██▒███   ▓██  ▀█ ██▒▒██░  ██▒▒ ▓██░ ▒░▒████ ░▒██░  ██▒▓██  ▒██░▓██  ▀█ ██▒░██   █▌\r\n▒▓▓▄ ▄██▒▓▓█  ░██░▒██░█▀  ▒▓█  ▄ ▓██▒  ▐▌██▒▒██   ██░░ ▓██▓ ░ ░▓█▒  ░▒██   ██░▓▓█  ░██░▓██▒  ▐▌██▒░▓█▄   ▌\r\n▒ ▓███▀ ░▒▒█████▓ ░▓█  ▀█▓░▒████▒▒██░   ▓██░░ ████▓▒░  ▒██▒ ░ ░▒█░   ░ ████▓▒░▒▒█████▓ ▒██░   ▓██░░▒████▓ \r\n░ ░▒ ▒  ░░▒▓▒ ▒ ▒ ░▒▓███▀▒░░ ▒░ ░░ ▒░   ▒ ▒ ░ ▒░▒░▒░   ▒ ░░    ▒ ░   ░ ▒░▒░▒░ ░▒▓▒ ▒ ▒ ░ ▒░   ▒ ▒  ▒▒▓  ▒ \r\n  ░  ▒   ░░▒░ ░ ░ ▒░▒   ░  ░ ░  ░░ ░░   ░ ▒░  ░ ▒ ▒░     ░     ░       ░ ▒ ▒░ ░░▒░ ░ ░ ░ ░░   ░ ▒░ ░ ▒  ▒ \r\n░         ░░░ ░ ░  ░    ░    ░      ░   ░ ░ ░ ░ ░ ▒    ░       ░ ░   ░ ░ ░ ▒   ░░░ ░ ░    ░   ░ ░  ░ ░  ░ \r\n░ ░         ░      ░         ░  ░         ░     ░ ░                      ░ ░     ░              ░    ░    \r\n░                       ░                                                                          ░      ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("CubeNotFound's Server Platform Shell");

            while (true)
            {
                Console.WriteLine("");
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input.StartsWith("LOGIN", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 3 || split.Length > 3)
                    {
                        Console.WriteLine("ERROR! Usage: LOGIN <username> <password>");
                    } else
                    {
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
                } else if (input.StartsWith("register", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 3 || split.Length > 3)
                    {
                        Console.WriteLine("ERROR! Usage: REGISTER <username> <password>");
                    }
                    else
                    {
                        string url = "http://localhost:5678/auth/register";

                        string json = $"{{\"username\":\"{split[1]}\",\"password\":\"{split[2]}\"}}";

                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.Accept = "application/json";
                        request.ContentType = "application/json";

                        byte[] data = Encoding.UTF8.GetBytes(json);
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        try
                        {
                            using (var response = (HttpWebResponse)request.GetResponse())
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                } else if (input.StartsWith("create", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 11 || split.Length > 11)
                    {
                        Console.WriteLine("ERROR! Usage: CREATE <name> <description> <software> <version> <minimum RAM> <maximum RAM> <Java version> <Java vendor> <Java type> <token>");
                    } else
                    {
                        string url = "http://localhost:5678/servers/create";

                        string json =
                            $"{{\"serverName\":\"{split[1]}\"," +
                            $"\"serverDesc\":\"{split[2]}\"," +
                            $"\"software\":\"{split[3]}\"," +
                            $"\"version\":\"{split[4]}\"," +
                            $"\"minRam\":\"{split[5]}\"," +
                            $"\"maxRam\":\"{split[6]}\"," +
                            $"\"javaVer\":\"{split[7]}\"," +
                            $"\"javaVendor\":\"{split[8]}\"," +
                            $"\"javaType\":\"{split[9]}\"}}";

                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.Accept = "application/json";
                        request.ContentType = "application/json";
                        request.Headers["Authorization"] = "Bearer " + split[10];

                        byte[] data = Encoding.UTF8.GetBytes(json);
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        try
                        {
                            using (var response = (HttpWebResponse)request.GetResponse())
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                } else if (input.StartsWith("logout", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 2 || split.Length > 2)
                    {
                        Console.WriteLine("ERROR! Usage: LOGOUT <token>");
                    } else
                    {
                        string url = "http://localhost:5678/auth/logout";

                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.Headers["Authorization"] = "Bearer " + split[1];
                        request.ContentLength = 0; // important: no body

                        try
                        {
                            using (var response = (HttpWebResponse)request.GetResponse())
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                } else if (input.StartsWith("getservers", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 2 || split.Length > 2)
                    {
                        Console.WriteLine("ERROR! Usage: GETSERVERS <token>");
                    }
                    else
                    {
                        string url = "http://localhost:5678/profile/servers";

                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "GET";
                        request.Headers["Authorization"] = "Bearer " + split[1];
                        request.ContentLength = 0; // important: no body

                        try
                        {
                            using (var response = (HttpWebResponse)request.GetResponse())
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                } else if (input.StartsWith("startserver", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 3 || split.Length > 3)
                    {
                        Console.WriteLine("ERROR! Usage: STARTSERVER <server ID> <token>");
                    }
                    else
                    {
                        string url = "http://localhost:5678/servers/start";

                        string json =
                            $"{{\"id\":\"{split[1]}\"," +
                            $"}}";

                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.Accept = "application/json";
                        request.ContentType = "application/json";
                        request.Headers["Authorization"] = "Bearer " + split[2];

                        byte[] data = Encoding.UTF8.GetBytes(json);
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        try
                        {
                            using (var response = (HttpWebResponse)request.GetResponse())
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                } else if (input.StartsWith("deleteserver", StringComparison.OrdinalIgnoreCase))
                {
                    string[] split = SplitArgsPreserveQuotes(input);

                    if (split.Length < 3 || split.Length > 3)
                    {
                        Console.WriteLine("ERROR! Usage: DELETESERVER <server ID> <token>");
                    }
                    else
                    {
                        string url = "http://localhost:5678/servers/delete";

                        string json =
                            $"{{\"id\":\"{split[1]}\"," +
                            $"}}";

                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.Accept = "application/json";
                        request.ContentType = "application/json";
                        request.Headers["Authorization"] = "Bearer " + split[2];

                        byte[] data = Encoding.UTF8.GetBytes(json);
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        try
                        {
                            using (var response = (HttpWebResponse)request.GetResponse())
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null)
                            {
                                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                } else if (input.Equals("clear", StringComparison.OrdinalIgnoreCase) || input.Equals("cls", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                } else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
            }
        }

        static string[] SplitArgsPreserveQuotes(string input)
        {
            var result = new List<string>();
            var current = new StringBuilder();

            bool inQuotes = false;
            bool escape = false;

            foreach (char c in input)
            {
                if (escape)
                {
                    // Always treat escaped char literally
                    current.Append(c);
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true;
                    continue;
                }

                if (c == '"')
                {
                    inQuotes = !inQuotes; // toggle quoting
                    continue;             // strip quotes
                }

                if (c == ' ' && !inQuotes)
                {
                    if (current.Length > 0)
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    current.Append(c);
                }
            }

            if (escape)
            {
                // trailing backslash, treat literally
                current.Append('\\');
            }

            if (current.Length > 0)
                result.Add(current.ToString());

            return result.ToArray();
        }
    }
}
