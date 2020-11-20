using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EmailService.Logic;
using EmailService.Logic.Database;
using EmailService.Logic.Saving;

namespace EmailService.Api
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
            ConfigureServicesForAssembly(services, typeof(EmailSaver).Assembly);
            ConfigureServicesForAssembly(services, typeof(EmailPersister).Assembly);

            services.AddScoped<EmailSaver>();
            services.AddControllers().AddNewtonsoftJson();

            var persister = new EmailPersister();
            services.AddSingleton<IEmailPersister>(persister);
            services.AddSingleton<IEmailDataReader>(persister);
        }

        private void ConfigureServicesForAssembly(IServiceCollection services, Assembly assembly)
        {
            var classes = assembly.GetTypes().Where(x => x.IsClass);
            foreach (var classType in classes)
            {
                var interfaces = classType.GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (interfaceType.Name == "ISerializable")
                    {
                        continue;
                    }
                    services.AddScoped(interfaceType, classType);
                }

                //if (interfaces.Length == 0)
                //{
                //    services.AddScoped(classType);
                //}
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
