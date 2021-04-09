using HttpClientPollyExample.Interfaces;
using HttpClientPollyExample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace HttpClientPollyExample
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
            services.AddHttpClient<IHttpBinService, HttpBinService>()
                .AddPolicyHandler(GetRetryPolicy(retryCount: 3))
                .AddPolicyHandler(GetCircuitBreakerPolicy(exceptionsAllowedBeforeBreaking: 5, durationOfBreakInSeconds: 30));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, int durationOfBreakInSeconds)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, TimeSpan.FromSeconds(durationOfBreakInSeconds));
        }
    }
}
