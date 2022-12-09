using Loan.Application.Commons;
using Loan.Application.Interfaces;
using Loan.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Loan.Application.Web.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Loan.Application.Web.API", Version = "v1" });
            });

            services.Configure<LocalNumbersOptions>(Configuration.GetSection("LocalNumbersOptions"));
            services.Configure<IndustryOptions>(Configuration.GetSection("IndustryOptions"));
            services.Configure<LoanAmountOptions>(Configuration.GetSection("LoanAmountOptions"));
            services.Configure<CitizenshipOptions>(Configuration.GetSection("CitizenshipOptions"));
            services.Configure<TradingTimeOptions>(Configuration.GetSection("TradingTimeOptions"));
            services.Configure<CountryCodeOptions>(Configuration.GetSection("CountryCodeOptions"));
            services.Configure<BusinessNumberOptions>(Configuration.GetSection("BusinessNumberOptions"));


            services.AddScoped<IPreAssessmentService, PreAssessmentService>();
            services.AddScoped<IValidationService, ValidationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan.Application.Web.API v1"));
            }

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
