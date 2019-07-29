using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Munro.Common.Invoke;
using Munro.Infrastructure.Contract.Repositories;
using Munro.Infrastructure.Repositories;
using Munro.Services;
using Munro.Services.Contract;
using Munro.Services.Contract.Helpers;
using Munro.Services.Contract.Services;
using Munro.Services.Helpers;
using Munro.Services.Services;

namespace Munro.Web
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
            services.AddScoped(typeof(IInvokeHandler<>), typeof(BaseInvokeHandler<>));
            services.AddSingleton<IInvokeResultSettings, InvokeResultSettings>();
            services.AddTransient<IExpressionBuilder, ExpressionBuilder>();
            services.AddSingleton<IMunrosRepository, MunrosRepository>();
            services.AddTransient<IMunrosManager, MunrosManager>();
            services.AddTransient<IMunroService, MunroService>();
            services.AddTransient<IFileReaderService, FileReaderService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}