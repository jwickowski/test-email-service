using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using EmailService.Api.CustomExceptionMiddleware;
using EmailService.Database;
using EmailService.Logic;
using EmailService.Logic.Saving;
using EmailService.Logic.Sending;
using EmailService.SMTP;

namespace EmailService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesForAssembly(services, typeof(EmailSaver).Assembly);
            ConfigureServicesForAssembly(services, typeof(EmailPersister).Assembly);
            ConfigureServicesForAssembly(services, typeof(SmtpSender).Assembly);

            services.AddScoped<EmailSender>();
            services.AddScoped<EmailSaver>();
            services.AddSingleton<CustomExceptionMapper>();
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
                    services.AddScoped(interfaceType, classType);
                }
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

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
