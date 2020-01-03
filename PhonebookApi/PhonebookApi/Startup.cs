using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PhonebookApi.DataStore;
using PhonebookApi.DTO;
using Unity;

namespace PhonebookApi
{
    public class Startup
    {
        public static IUnityContainer Container { get; set; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Container = new UnityContainer();
            RegisterServices();
            
        }

        public static IWebHostEnvironment Environment { get; set; }
        
        private void RegisterServices()
        {
            Container.RegisterType<IPhonebookDataStore, PhonebookDataStore>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PhonebookContext>();
            services.AddScoped<IPhonebookDataStore, PhonebookDataStore>();
            
            services.AddControllers();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo {Title = "Phonebook Api", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Phonebook Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}