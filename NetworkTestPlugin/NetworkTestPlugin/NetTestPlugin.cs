using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace NetworkTestPlugin
{
    public class NetTestPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var url = context.InputParameters["TargetUrl"].ToString();
            
            var response = new StringBuilder();

            response.AppendLine("Dns Resolution");
            response.AppendLine("==============");

            IPHostEntry hostEntry = Dns.GetHostEntry(url);

            response.AppendLine("Host Name: " + hostEntry.HostName);
            response.AppendLine("IP Addresses:");

            foreach (IPAddress ip in hostEntry.AddressList)
            {
                response.AppendLine(ip.ToString());
            }
            response.AppendLine("Tracing Route...");
            TraceRoute(hostEntry.AddressList.FirstOrDefault()?.ToString(), response);

            context.OutputParameters.AddOrUpdateIfNotNull("Response", response.ToString());

            //response.AppendLine();
            //response.AppendLine("Ping Test");
            //response.AppendLine("=========");



            //using (HttpClient client = new HttpClient())
            //{
            //    var response = client.GetAsync(url).Result;
            //    var result = response.Content.ReadAsStringAsync().Result;
            //    context.OutputParameters.AddOrUpdateIfNotNull("Response", result);
            //}
        }

        private static void TraceRoute(string hostNameOrAddress, StringBuilder response, int ttl = 1)
        {
            Ping pinger = new Ping();
            PingOptions pingerOptions = new PingOptions(ttl, true);
            var timeout = 5000;
            byte[] buffer = Encoding.ASCII.GetBytes(new string('a', 128));

            try
            {
                PingReply reply = pinger.Send(hostNameOrAddress, timeout, buffer, pingerOptions);

                if (reply.Status == IPStatus.Success)
                {
                    response.AppendLine($"{ttl} ==> {reply.Address}");
                }
                else if (reply.Status == IPStatus.TtlExpired || reply.Status == IPStatus.TimedOut)
                {
                    if (reply.Status == IPStatus.TtlExpired)
                        response.AppendLine($"{ttl} ==> {reply.Address}");


                    if (ttl < 10)
                    {
                        TraceRoute(hostNameOrAddress, response, ttl + 1);
                    }
                }
                response.AppendLine($"{ttl} ==> {reply.Status}");
            }
            catch (Exception e)
            {
                response.AppendLine($"ping failed: {e.Message}");
            }
        }
    }
}
