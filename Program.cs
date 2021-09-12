using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace netcore_kestrel_server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var socketOptions = Options.Create(new SocketTransportOptions());
            
            var serverOptions = Options.Create(new KestrelServerOptions()
            {
                ApplicationServices = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
            });

            var server = new KestrelServer(serverOptions
            , new SocketTransportFactory(socketOptions, NullLoggerFactory.Instance)
            , NullLoggerFactory.Instance);

            await server.StartAsync(new Application(),CancellationToken.None);

            Console.Read();


        }


    }

    public class Application : IHttpApplication<HttpContext>
    {
        public HttpContext CreateContext(IFeatureCollection contextFeatures)
        {
            return new DefaultHttpContext(contextFeatures);
        }

        public void DisposeContext(HttpContext context, Exception exception)
        {

        }

        public Task ProcessRequestAsync(HttpContext context)
        {
            return context.Response.WriteAsync("Hello World");
        }
    }
}
