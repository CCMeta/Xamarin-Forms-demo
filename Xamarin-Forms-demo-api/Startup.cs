using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Xamarin_Forms_demo_api.Models;
using System.Text.Encodings.Web;
using Xamarin_Forms_demo_api.Services;
using System.Reflection;
using System;
using Microsoft.AspNetCore.SignalR;

namespace Xamarin_Forms_demo_api
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
            services.AddSingleton<SessionService>()
                .AddTransient<KnowledgesRepository>()
                .AddTransient<ExamTranscriptsRepository>()
                .AddTransient<ExamAnswersRepository>()
                .AddTransient<ExamQuestionsRepository>()
                .AddTransient<ExamsRepository>()
                .AddTransient<SubjectsRepository>()
                .AddTransient<CoursesRepository>()
                .AddTransient<ContactsRepository>()
                .AddTransient<ChatsRepository>()
                .AddTransient<UsersRepository>()
                .AddTransient<PostsRepository>();
            services.AddSingleton<IUserIdProvider, ChatHubUserProvider>();
            services.AddSingleton<ChatHub>();
            services.AddSignalR();
            services.AddHttpContextAccessor();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Xamarin_Forms_demo_api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Xamarin_Forms_demo_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            //app.UseResponseCaching();
            //app.UseResponseCompression();
            //app.UseStaticFiles();

            //SESSION 
            app.UseMiddleware<AuthMiddleware>();
            //SESSION CHECK
            app.UseWhen(context => context.Request.Path.Value.ToLower().Contains("/api") && context.Request.Path.Value.ToLower() != "/api/token",
                static (IApplicationBuilder app) =>
            {
                app.Use(async (context, next) =>
                {
                    if (!context.Items.ContainsKey("uid"))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.CompleteAsync();
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapControllers();
            });
        }
    }
}
