using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTOs;
using Common.DTOs.Enums;
using Common.Helper;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RestSharp;
using RestSharp.Authenticators;

namespace AgentService
{
    public class MapWorker
    {
        private ILogger<MapWorker> _logger;
        private IConnection _connection;
        private IModel _channel;
        private CrawlData _lastOne;
        private bool _readingOpenPorts = false;
        public MapWorker(ILogger<MapWorker> logger)
        {
            _logger = logger;
            var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "pass" };

            _connection = factory.CreateConnection();

        }

        private IModel Channel
        {
            get
            {
                if (_channel == null || _channel.IsClosed)
                {
                   _channel = _connection.CreateModel();
                   _channel.QueueDeclare(queue: "crawled",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
                }
                return _channel;
            }
        }

        private async Task<string> getNextBlock()
        {
            var url = "http://controlcyserver/segment";
            var client = new RestClient(url);
            client.Authenticator = new HttpBasicAuthenticator("xTremeMustafa", "Master1312");

            var request = new RestRequest(url, Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content.Replace("\"", "");
        }
        private async void Map(string ip)
        {
            Console.WriteLine("starting scan for {0} thread no{1}", ip, Thread.CurrentThread.ManagedThreadId);
            Process sortProcess = new Process();
            sortProcess.StartInfo.FileName = "nmap";
            sortProcess.StartInfo.Arguments = String.Format("{0} -F -v", ip);
            // Set UseShellExecute to false for redirection.
            sortProcess.StartInfo.UseShellExecute = false;

            // Redirect the standard output of the sort command.
            // This stream is read asynchronously using an event handler.
            sortProcess.StartInfo.RedirectStandardOutput = true;
            sortProcess.StartInfo.RedirectStandardError = true;
            // Set our event handler to asynchronously read the sort output.
            sortProcess.OutputDataReceived += SortOutputHandler;
            sortProcess.ErrorDataReceived += SortOutputHandler;
            // Start the process.
            sortProcess.Start();

            // Use a stream writer to synchronously write the sort input.
            // Start the asynchronous read of the sort output stream.
            sortProcess.BeginOutputReadLine();
            sortProcess.BeginErrorReadLine();


            // Wait for the sort process to write the sorted text lines.
            sortProcess.WaitForExit();
            sortProcess.Close();
        }

        private void SendLastOne()
        {
            if (_lastOne != null)
            {
                if (_lastOne.Ports.Count > 0)
                {
                    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_lastOne));
                    Channel.BasicPublish("", "crawled", null, messageBodyBytes);
                }
                _lastOne = null;
            }
        }
        private void SortOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                if (outLine.Data.StartsWith("Nmap scan report for"))
                {
                    SendLastOne();
                    var rawString = outLine.Data.Replace("Nmap scan report for ", "");
                    _lastOne = new CrawlData();
                    _lastOne.IPAddress = rawString.Trim();
                    _readingOpenPorts = true;
                }
                else if (outLine.Data.StartsWith("Host is up") || outLine.Data.StartsWith("Not shown:") || outLine.Data.StartsWith("PORT"))
                {
                }
                else if (char.IsDigit(outLine.Data[0]))
                {
                    var split = outLine.Data.Split(' ', StringSplitOptions.RemoveEmptyEntries  |StringSplitOptions.TrimEntries);
                    var moresplit = split[0].Split('/', StringSplitOptions.TrimEntries);
                    var status = (PortStatus)Enum.Parse(typeof(PortStatus), split[1]);
                    var tcpudp = (TcpUdp)Enum.Parse(typeof(TcpUdp), moresplit[1]);
                    if(status != PortStatus.closed )
                    _lastOne.Ports.Add(new PortData { Port = ushort.Parse(moresplit[0]), Status = status, TcpUdp = tcpudp });
                }
                else
                {


                }
                Console.WriteLine(outLine.Data);
            }
        }


        public void Run()
        {
            while (true)
            {
                string nextBlock = getNextBlock().Result;
                List<string> divided = CIDRDivider.Divide(nextBlock);
                foreach (var div in divided)
                {
                    Map(div);
                }

            }
        }
    }

}
