using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using AmazonWebApplication1.Database;
using AmazonWebApplication1.Models;
using AmazonWebApplication1.Services;
using AmazonWebApplication1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;


namespace AmazonWebApplication1
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmazonWebApplication1", Version = "v1" });
            });

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSQS>();
            IServiceCollection serviceCollection = services.AddAWSService<IAmazonSimpleNotificationService>();

            services.AddTransient<IRDSService, RDSService>();
            services.AddTransient<ISNSService, SNSService>();
            services.AddTransient<ISQSService, SQSService>();
            services.AddTransient<IS3Service, S3Service>();
            services.AddTransient<IMessageService, MessageService>();

            services.AddCors();
            services.AddDbContext<RDSContext>(options =>
                options.UseMySql("Server=rds.cyqc7gmapyp3.us-west-2.rds.amazonaws.com;Port=3306;Database=KProject;Uid=root;Password=li6482bam6809;",
                    new MySqlServerVersion(new Version(8, 0, 23)),
                    mySqlOptionsAction: MySqlDbContextOptionsBuilderExtensions =>
                    {
                        MySqlDbContextOptionsBuilderExtensions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                    }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmazonWebApplication1 v1"));
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
