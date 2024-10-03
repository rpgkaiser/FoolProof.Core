using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoolProof.Core.Tests.E2eTests.WebApp
{
    public class CustomWebAppFactory: WebApplicationFactory<Program>
    {
        private readonly int listeningPort;
        
        public CustomWebAppFactory(int listeningPort)
        {
            this.listeningPort = listeningPort;
        }

        public IHost? AppHost { get; set; }

        public string ServerAddress
        {
            get
            {
                EnsureServer();
                return ClientOptions.BaseAddress.ToString();
            }
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Create the host for TestServer now before we  
            // modify the builder to use Kestrel instead.    
            var testHost = builder.Build();

            // Modify the host builder to use Kestrel instead  
            // of TestServer so we can listen on a real address.    

            builder.ConfigureWebHost(
                webHostBuilder => webHostBuilder.UseKestrel(
                    opts => {
                        opts.ListenLocalhost(listeningPort);
                    }
                )
            );

            // Create and start the Kestrel server before the test server,  
            // otherwise due to the way the deferred host builder works    
            // for minimal hosting, the server will not get "initialized    
            // enough" for the address it is listening on to be available.    
            // See https://github.com/dotnet/aspnetcore/issues/33846.    

            AppHost = builder.Build();
            AppHost.Start();

            // Extract the selected dynamic port out of the Kestrel server  
            // and assign it onto the client options for convenience so it    
            // "just works" as otherwise it'll be the default http://localhost    
            // URL, which won't route to the Kestrel-hosted HTTP server.     

            var server = AppHost.Services.GetRequiredService<IServer>();
            var addresses = server.Features.Get<IServerAddressesFeature>();

            ClientOptions.BaseAddress = addresses!.Addresses
                .Select(x => new Uri(x))
                .Last();

            // Return the host that uses TestServer, rather than the real one.  
            // Otherwise the internals will complain about the host's server    
            // not being an instance of the concrete type TestServer.    
            // See https://github.com/dotnet/aspnetcore/pull/34702.   

            testHost.Start();
            return testHost;
        }

        private void EnsureServer()
        {
            if (AppHost is null)
            {
                // This forces WebApplicationFactory to bootstrap the server  
                using var _ = CreateDefaultClient();
            }
        }
    }
}
