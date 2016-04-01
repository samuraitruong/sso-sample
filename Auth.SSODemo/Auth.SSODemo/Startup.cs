using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using IdentityServer3.Core.Configuration;
using IdentityServer3;
using IdentityServer3.Core.Services.InMemory;

[assembly: OwinStartup(typeof(Auth.SSODemo.Startup))]

namespace Auth.SSODemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            var factory = new IdentityServerServiceFactory();
            factory
                .UseInMemoryClients(Config.GetClients())
                .UseInMemoryScopes(Config.GetScopes())
                .UseInMemoryUsers(Config.GetUsers().ToList());

            var options = new IdentityServerOptions
            {
                SiteName = "IdentityServer3 (self host)",
                RequireSsl=false,

                SigningCertificate = LoadCertificate(),
                Factory = factory,
            };

            app.UseIdentityServer(options);

        }
        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\Config\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}