using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Services;
using System.Text;

namespace PluginNetDiag
{
    /// <summary>
    /// Plugin development guide: https://docs.microsoft.com/powerapps/developer/common-data-service/plug-ins
    /// Best practices and guidance: https://docs.microsoft.com/powerapps/developer/common-data-service/best-practices/business-logic/
    /// </summary>
    public class PlexNetDiag : PluginBase
    {
        public PlexNetDiag(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(PlexNetDiag))
        {
            // TODO: Implement your custom configuration handling
            // https://docs.microsoft.com/powerapps/developer/common-data-service/register-plug-in#set-configuration-data
        }

        // Entry point for custom business logic execution
        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            if (localPluginContext == null)
            {
                throw new ArgumentNullException(nameof(localPluginContext));
            }

            var context = localPluginContext.PluginExecutionContext;
            var tracingService = localPluginContext.TracingService;

            if (context.MessageName.Equals("plex_NetDiag") && context.Stage.Equals(30))
            {
                try
                {
                    string target = (string)context.InputParameters["hostOrIP"];

                    StringBuilder sb = new StringBuilder();
                    TraceRt("www.bing.com", s => sb.AppendLine(s));

                    context.InputParameters["TraceResult"] = new string[] { sb.ToString() };
                }
                catch (Exception ex)
                {
                    tracingService.Trace("NetDiag Error: {0}", ex.ToString());
                    throw new InvalidPluginExecutionException("An error occurred in plex_NetDiag.", ex);
                }
            }

            // Check for the entity on which the plugin would be registered
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity
                && entity.LogicalName == "cra59_netdiag" && entity.TryGetAttributeValue("cra59_name", out string hostNameOrIP))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    TraceRt(hostNameOrIP, s => sb.AppendLine(s));

                    entity.Attributes.Add("cra59_result", entity.LogicalName);

                }
                catch (Exception ex)
                {
                    tracingService.Trace("NetDiag Error: {0}", ex.ToString());
                    throw new InvalidPluginExecutionException("An error occurred in plex_NetDiag.", ex);
                }
            }
            else
            {
                throw new InvalidPluginExecutionException($"NetDiag Plugin was not registered on the expected message.");
            }
            
        }

        void TraceRt(string hostOrIP, Action<string> writeLine)
        {
            var target = TraceEntry.From(hostOrIP);

            if (!target.IsFromIP && !target.IsValidEntry)
            {
                writeLine($"Cannot resolve {target}");
                return;
            }

            const int MaxTtl = 30;
            const int Timeout = 5000;

            writeLine($"Tracing route for: {target}");
            writeLine($".. over a max of 30 hops.");
            writeLine("");

            byte[] bytes = new byte[32];
            new Random().NextBytes(bytes);

            var ping = new Ping();
            for (int i = 1; i <= MaxTtl; i++)
            {
                var reply = ping.Send(target.Address, Timeout, bytes, new PingOptions(i, true));
                TraceEntry hop = TraceEntry.From(reply.Address);

                switch (reply.Status)
                {
                    case IPStatus.TtlExpired:
                        writeLine($"{i,2} > {hop}");
                        break;
                    case IPStatus.Success:
                        writeLine($"{i,2} > {target}");
                        writeLine($"Trace complete. Roundtrip time: {reply.RoundtripTime} ms.");
                        return;
                    default:
                        writeLine($"{i,2} > ** {reply.Status} **");

                        if (reply.Status != IPStatus.TimedOut)
                        {
                            return;
                        }
                        break;
                }
            }
        }

        class TraceEntry
        {
            private readonly string _hostName;
            private readonly IPAddress _ip;
            private bool _fromIP;

            public static TraceEntry From(string hostOrIP)
            {
                if (IPAddress.TryParse(hostOrIP, out var ip))
                {
                    return new TraceEntry(ip);
                }
                else
                {
                    return new TraceEntry(hostOrIP);
                }
            }
            public static TraceEntry From(IPAddress ip)
            {
                return new TraceEntry(ip);
            }

            private TraceEntry(string host)
            {
                _hostName = host;
                _fromIP = false;
                try
                {
                    var reply = Dns.GetHostEntry(_hostName);

                    _ip = reply.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);

                    if (_ip == null)
                    {
                        _ip = reply.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetworkV6);
                    }
                }
                catch (SocketException) { }
            }

            private TraceEntry(IPAddress ip)
            {
                _ip = ip;
                _fromIP = true;
                try
                {
                    var reply = Dns.GetHostEntry(_ip);
                    _hostName = reply.HostName;
                }
                catch (SocketException) { }
            }

            public bool IsValidEntry => _hostName != null && _ip != null;

            public IPAddress Address => _ip;

            public string HostName => _hostName;

            public bool IsFromIP => _fromIP;

            public override string ToString()
            {
                if (_hostName == null || _ip == null)
                {
                    return $"{_hostName ?? _ip?.ToString()}";
                }
                else
                {
                    return $"{_hostName} [{_ip}]";
                }
            }
        }
    }
}
