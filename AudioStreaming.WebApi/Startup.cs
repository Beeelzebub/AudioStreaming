using AudioStreaming.Application;
using AudioStreaming.Infrastructure;
using AudioStreaming.Persistence;
using Hangfire;
using Hangfire.Dashboard;
using MusicStreaming.Security;

namespace AudioStreaming.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.ConfigureSwagger();
            services.AddLogging();

            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);
            services.AddEntityFramework(Configuration);
            services.AddSecurityServices();
            services.ConfigureJwt(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            RecurringJob.RemoveIfExists("ChartUpdater");
            //RecurringJob.AddOrUpdate<ChartUpdater>("ChartUpdater", x => x.Update(), Configuration["Hangfire:ChartUpdaterCronExpression"]);

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new List<IDashboardAuthorizationFilter>()
            });

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
