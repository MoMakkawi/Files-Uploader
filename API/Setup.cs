using System.Text;

using Application.Contracts;
using Application.Mappers;
using Application.Services;

using AutoMapper;


using Hangfire;
using Hangfire.MySql;

using Infrastructure.BackgroundServices;
using Infrastructure.Options;
using Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Persistence.Data;
using Persistence.Services.Repositories;


namespace API;

public static class Setup
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapperService();
        builder.Services.AddHangFireServices();

        builder.Services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        builder.Services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();

        builder.Services.AddScoped<IAttachmentServiceAsync, AttachmentServiceAsync>();
        
        builder.Services.AddScoped<IAuthenticationServiceAsync, AuthenticationServiceAsync>();
        builder.Services.AddScoped<IEmailSenderServiceAsync, EmailSenderServiceAsync>();
    }

    public static void AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.AddAuthenticationConfigurations();
        builder.AddDBConfiguration();
        builder.AddBackgroundServiceDBConfiguration();
        builder.AddEmailConfiguration();
    }
    public static void AddBackgroundServices()
    {
        RecurringJob.AddOrUpdate<RemainderEmailService>(
        "send-remainders-emails-to-HasNotFirstLoginUsers",
        service => service.SendForHasNotLoginUsersBeforeAsync(),
        Cron.Daily(17, 0) // Run every day at 5:00 PM (17:00)
    );
    }

    #region Services
    private static void AddAutoMapperService(this IServiceCollection services)
        => new List<IMapper>
        {
            new MapperConfiguration(mc => mc.AddProfile(new UserProfile())).CreateMapper(),
            new MapperConfiguration(mc => mc.AddProfile(new AttachmentProfile())).CreateMapper()
        }
        .ForEach(mapper => services.AddSingleton(mapper));

    private static void AddHangFireServices(this IServiceCollection services)
    {
        services.AddHangfireServer();
        services.AddScoped<RemainderEmailService>();
    }
    #endregion
    #region Configs
    private static void AddDBConfiguration(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("MySQLConnectionString");
        builder.Services.AddDbContext<MySQLDBContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }
    private static void AddBackgroundServiceDBConfiguration(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("BackgroundServiceConnectionString");
        // Configure HangFire with MySQL storage
        builder.Services.AddHangfire(config => config
            .UseStorage(new MySqlStorage(connectionString,
                            new MySqlStorageOptions
                            {
                                TablesPrefix = "HangFire"
                            })));

    }
    private static void AddEmailConfiguration(this WebApplicationBuilder builder)
    {
        var emailConfig = builder.Configuration.GetSection("EmailSettings");
        builder.Services.Configure<EmailSettings>(emailConfig);
    }

    private static void AddAuthenticationConfigurations(this WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        builder.Services.AddSingleton(jwtOptions);

        var jwtService = new AuthenticationServiceAsync(jwtOptions);
        builder.Services.AddSingleton<IAuthenticationServiceAsync>(jwtService);

        builder.Services.AddAuthentication()
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
            };
        });
    }
    #endregion
}
