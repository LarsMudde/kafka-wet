﻿using AspNetCore.Extensions.Streaming.Configuration;
using AspNetCore.Extensions.Streaming.Consumer;
using AspNetCore.Extensions.Streaming.Publisher;
using Kafka_WET.Domain.Events;
using Kafka_WET.Services.Service;
using Kafka_WET.Services.Streaming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Kafka_WET
{
    public class Startup
    {

        // Enable or disable event listeners
        private bool enableEventlisteners;
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            enableEventlisteners = configuration.GetValue<bool>("KafkaListenerEnabled");
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Configure services
            services.AddTransient<ISubscriptionService, SubscriptionService>();

            // Streaming configuration
            services.Configure<KafkaConfig>(Configuration.GetSection(nameof(KafkaConfig)));

            // Streaming event listeners
            if (enableEventlisteners)
            {
                services.AddSingleton<IConsumer<SubscriptionEvent>, Consumer<SubscriptionEvent>>();
                services.AddHostedService<SubscriptionEventListener>();
            }

            // Streaming publisher
            services.AddSingleton<IPublisher<SubscriptionEvent>, Publisher<SubscriptionEvent>>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WET-Kafka", Version = "v1" });
            });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
