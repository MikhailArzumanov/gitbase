using Backend.Constants;
using Backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Backend.Configuring {
    #region Класс ServicesConfigurator
    /** Класс ServicesConfigurator
     * <summary>
     *  Класс нацелен на осуществление предварительной конфигурации сервисов приложения.
     * </summary>
     */
    public class ServicesConfigurator {
        #region Поля
        /** Поле Configuration
         * <summary>
         *  Поле интерфейса конфигурации.
         * </summary>
         */
        private IConfiguration Configuration { get; set; }
        #endregion
        #region Конструктор
        /** Конструктор ServicesConfigurator(IConfiguration)
         * <summary>
         *  Осуществляет задание контекста интерфейса конфигурации.
         * </summary>
         * <param name="configuration">
         *  Интерфейс конфигурации.
         * </param>
         */
        private ServicesConfigurator(IConfiguration configuration) {
            Configuration = configuration;
        }
        #endregion
        #region Частное конфигурирование
        /** Функция ConfigurePostgreSQL(IServiceCollection)
         * <summary>
         *  Функция осуществляет конфигурирование интерфейса СУБД PostgreSQL.<br/>
         *  <br/>
         *  Устанавливается строка соединения, <br/>
         *  взымаемая из json-файла конфигурации приложения.<br/>
         *  <br/>
         *  Устанавливается связь СУБД и контекста приложения.
         * </summary>
         * <param name="services">Коллекция сервисов.</param>
         */
        private void ConfigurePostgreSQL(IServiceCollection services) {
            var postgresConnectionString =
                Configuration.GetConnectionString(
                    Constants.Configuration.POSTGRESQL_CONNECTION_STRING_KEY
                );
            services.AddDbContext<ApplicationContext>(
                options => options.UseNpgsql(postgresConnectionString)
            );
        }
        /** Функция ConfigureCors(IServiceCollection)
         * <summary>
         *  Функция осуществляет конфигурирование допущения CORS-запросов.<br/>
         * </summary>
         * <param name="services">Коллекция сервисов.</param>
         */
        private void ConfigureCors(IServiceCollection services) {
            var corsPolicy = Cors.APPLICATION_CORS_POLICY;
            services.AddCors(o =>
                o.AddPolicy(corsPolicy, builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                })
            );
        }
        /** Функция ConfigureSerialization(IServiceCollection)
         * <summary>
         *  Функция осуществляет конфигурирование сериализации.<br/>
         *  Устанавливается игнорирование циклических ссылок.
         * </summary>
         * <param name="services">Коллекция сервисов.</param>
         */
        private void ConfigureSerialization(IServiceCollection services) {
            services.AddMvcCore().AddJsonOptions(options => {
                options.JsonSerializerOptions.ReferenceHandler = 
                    ReferenceHandler.IgnoreCycles;
            });
        }

        private void ConfigureAuthentication(IServiceCollection services) {
            var authScheme = JwtBearerDefaults.AuthenticationScheme;

            var jwtKeyStr = Configuration[Constants.Configuration.JWT_KEY_CONFIGURATION_KEY];
            if(jwtKeyStr == null) {
                throw new Exception(ErrMsgs.JWT_CONFIG_IS_NOT_VALID);
            }
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKeyStr);
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes)
            };

            services.AddAuthentication(authScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = tokenValidationParameters;
            });
        }

        /** Функция ConfigureSwaggerService(IServiceCollection)
         * <summary>
         *  Функция осуществляет конфигурирование спецификации OpenAPI.<br/>
         *  Устанавливается заголовок, версия и учёт конечных точек и контроллеров.
         * </summary>
         * <param name="services">Коллекция сервисов.</param>
         */
        private void ConfigureSwaggerService(IServiceCollection services) {
            var infoObject = new Microsoft.OpenApi.Models.OpenApiInfo {
                Title   = Application.APP_TITLE   ,
                Version = Application.APP_VERSION ,
            };

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", infoObject);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT token into field."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{ {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
        #endregion
        #region Общее конфигурирование
        /** Функция Configure(IServiceCollection, IConfiguration)
         * <summary>
         *  Функция инкапсулирует все действия 
         *  для предварительного конфигурирования сервисов.<br/>
         *  Осуществляется конфигурация интерфейса СУБД PostgreSQL,<br/>
         *  политики CORS, сериализации и генерируемой спецификации OpenAPI.
         * </summary>
         * <param name="services">Интерфейс коллекции сервисов.</param>
         */
        private void Configure(IServiceCollection services) {
            ConfigurePostgreSQL(services);

            ConfigureCors(services);
            ConfigureSerialization(services);
            ConfigureAuthentication(services);
            ConfigureSwaggerService(services);
        }
        /** Функция Configure(IServiceCollection,IConfiguration)
         * <summary>
         *  Статическая функция создаёт экземпляр класса, 
         *  вызывая его внутренний функционал.<br/>
         *  См. <see cref="Configure(IServiceCollection)"/>.
         * </summary>
         * <param name="services">Интерфейс коллекции сервисов.</param>
         * <param name="configuration">Интерфейс конфигурации приложения</param>
         */
        public static void Configure(
            IServiceCollection services,
            IConfiguration configuration
        ) {
            var instance = new ServicesConfigurator(configuration);
            instance.Configure(services);
        }
        #endregion
    }
    #endregion
}
