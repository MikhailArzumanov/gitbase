using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using GitbaseBackend.Data;

namespace GitbaseBackend {
    public class Startup : StartupBase {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ///////////////////////////////////////////////////////////// //
        //      Данный метод подлежит вызову во времени выполнения.      //
        // Используйте данный метод для добавления сервисов в контейнер. //
        // ///////////////////////////////////////////////////////////// //
        override public void ConfigureServices(IServiceCollection services) {
            var postgresConnectionString = Configuration.GetConnectionString("PostgresqlConnection");
            services.AddDbContext<ApplicationContext>(
                options => options.UseNpgsql(postgresConnectionString)
            );

            services.AddCors(o => o.AddPolicy("CorsAllowAny", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            var jwtKeyBytes = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => {
                   options.TokenValidationParameters = new TokenValidationParameters {
                       ValidateIssuer           = false ,
                       ValidateAudience         = false ,
                       ValidateLifetime         = true  ,
                       ValidateIssuerSigningKey = true  ,

                       IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes)
                   };
               });

            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddControllers();
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            /*
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
            });
            */
        }

        // /////////////////////////////////////////////////////////////// //
        //       Данный метод подлежит вызову во времени выполнения.       //
        // Используйте данный метод для настройки процессов HTTP-запросов. //
        // /////////////////////////////////////////////////////////////// //
        override public void Configure(IApplicationBuilder app) {
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            //app.UseMvc();
        }
    }
}
