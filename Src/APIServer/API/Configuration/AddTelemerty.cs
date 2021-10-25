using System;
using APIServer.Domain;
using OpenTelemetry.Trace;
using System.Threading.Tasks;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SharedCore.Aplication.Services;
using SharedCore.Aplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddTelemerty(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment) {

            serviceCollection.AddOpenTelemetryTracing((builder) => {
                // Sources
                builder.AddSource(Sources.DemoSource.Name);

                builder.SetResourceBuilder(ResourceBuilder
                  .CreateDefault()
                //   .AddAttributes( new List<KeyValuePair<String, object>> { 
                //     new KeyValuePair<String, object>("SomeKey", "This is String Value")
                //     })
                  .AddService(Environment.ApplicationName));

                builder.AddAspNetCoreInstrumentation(opts => {
                    opts.RecordException = true;
                    opts.Enrich = async (activity, eventName, rawObject) =>
                    {

                        await Task.CompletedTask;
                        
                        if (eventName.Equals("OnStartActivity"))
                        {
                            if (rawObject is HttpRequest {Path: {Value: "/graphql"}})
                            {
                                // Do something else
                            }
                        }
                    };
                    // opts.Filter = (httpContext) =>
                    // {
                    //     // only collect telemetry about HTTP GET requests
                    //     // return httpContext.Request.Method.Equals("GET");
                    //     // return httpContext.Request.Path.Value != "/graphql";
                    // };
                });

                builder.AddElasticsearchClientInstrumentation();

                builder.AddSqlClientInstrumentation();

                builder.AddHttpClientInstrumentation(
                    opts => opts.RecordException = true);

                builder.AddEntityFrameworkCoreInstrumentation(
                    e => e.SetDbStatementForText = true);

                builder.AddOtlpExporter(options => {
                    options.Endpoint = new Uri(Configuration["ConnectionStrings:OtelCollector"]); // Export to collector
                    // options.Endpoint = new Uri("http://localhost:8200"); // Export dirrectly to APM
                    // options.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>() {
                    // };                
                });

                // if (Uri.TryCreate(Configuration.GetConnectionString("Jaeger"), UriKind.Absolute, out var uri)) {
                //     builder.AddJaegerExporter(opts => {
                //         opts.AgentHost = uri.Host;
                //         opts.AgentPort = uri.Port;
                //         opts.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>() {
                //         };
                //     });

                //     // builder.AddZipkinExporter(opts => {
                //     //     opts.Endpoint = new Uri("http://localhost:9412/api/v2/spans");
                //     // });
                // }
            });

            serviceCollection.AddSingleton<ITelemetry,Telemetry>();

            return serviceCollection;
        }
    }
}