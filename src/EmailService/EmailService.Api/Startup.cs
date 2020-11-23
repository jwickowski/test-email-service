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
using EmailService.Database;
using EmailService.Logic;
using EmailService.Logic.Saving;
using EmailService.Logic.Sending;
using EmailService.SMTP;

namespace EmailService.Api
{
    public class Startup
    {
        List<string> sss = new List<string>();
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
            ConfigureServicesForAssembly(services, typeof(SmtpSender).Assembly);

            services.AddScoped<EmailSender>();
            services.AddScoped<EmailSaver>();
            services.AddControllers().AddNewtonsoftJson();

            var persister = new EmailPersister();
            services.AddSingleton<IEmailPersister>(persister);
            services.AddSingleton<IEmailDataReader>(persister);
            services.AddSingleton<IPendingEmailsGetter>(persister);

            string defaultSenderMail = Configuration["EmailConfig:DefaultSenderMail"];
            services.AddSingleton<IEmailSenderConfig>(new EmailConfigResolver(defaultSenderMail));
        }

        private void ConfigureServicesForAssembly(IServiceCollection services, Assembly assembly)
        {
            var classes = assembly.GetTypes().Where(x => x.IsClass && x.IsPublic);
            foreach (var classType in classes)
            {
                if (classType.Name == "EmailPersister")
                {
                    continue;
                }
                var interfaces = classType.GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (interfaceType.Name == "ISerializable")
                    {
                        continue;
                    }
                    sss.Add($"{interfaceType.Name}=>{classType.Name}");
                    services.AddScoped(interfaceType, classType);
                }
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
