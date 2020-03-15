using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Extensions.Caching;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CouchbaseDistributedCacheSample.Repository;

namespace CouchbaseDistributedCacheSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<ILocationRepository, LocationRepository>();

            services.AddCouchbase(opt =>
            {
                opt.Servers = new List<Uri>
                    {
                        new Uri(Configuration.GetSection("Couchbase").GetValue<string>("Server"))
                    };
                opt.Username = Configuration.GetSection("Couchbase").GetValue<string>("Username");
                opt.Password = Configuration.GetSection("Couchbase").GetValue<string>("Password");
            });

            services.AddDistributedCouchbaseCache(Configuration.GetSection("Couchbase").GetValue<string>("BucketName"), opt => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
