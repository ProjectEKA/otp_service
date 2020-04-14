namespace In.ProjectEKA.OtpService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Clients;
    using Common;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Otp;
    using Otp.Model;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Notification;
    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddDbContext<OtpContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")))
                .AddSingleton<ISmsClient, SmsClient>()
                .AddSingleton(new OtpProperties(Configuration.GetValue<int>("expiryInMinutes")))
                .AddScoped<IOtpRepository, OtpRepository>()
                .AddScoped<IOtpGenerator, OtpGenerator>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<OtpVerifier, OtpVerifier>()
                .AddScoped<OtpSender, OtpSender>()
                .AddScoped<FakeOtpSender, FakeOtpSender>()
                .AddScoped(serviceProvider =>
                    new OtpSenderFactory(serviceProvider.GetService<OtpSender>(),
                        serviceProvider.GetService<FakeOtpSender>(),
                        Configuration.GetValue<string>("whitelisted:numbers")?.Split(",").ToList()))
                .AddRouting(options => options.LowercaseUrls = true)
                .AddControllers()
                .AddNewtonsoftJson(options => { })
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // Need to validate Audience and Issuer properly
                    options.Authority = Configuration.GetValue<string>("authServer:url");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        AudienceValidator = (audiences, token, parameters) => true,
                        IssuerValidator = (issuer, token, parameters) => token.Issuer
                    };
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            const string claimTypeClientId = "clientId";
                            if (!context.Principal.HasClaim(claim => claim.Type == claimTypeClientId))
                            {
                                context.Fail($"Claim {claimTypeClientId} is not present in the token.");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting()
                .UseStaticFilesWithYaml()
                .UseCustomOpenAPI()
                .UseIf(!env.IsDevelopment(), x => x.UseHsts())
                .UseIf(env.IsDevelopment(), x => x.UseDeveloperExceptionPage())
                .UseSerilogRequestLogging()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var otpContext = serviceScope.ServiceProvider.GetService<OtpContext>();
            otpContext.Database.Migrate();
        }
    }
}