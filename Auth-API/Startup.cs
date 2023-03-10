using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Auth_API.Data;
using Auth_API.Helpers;
using Auth_API.Models;
using Auth_API.Services;
using Auth_API.Services.Auth;
using Auth_API.Services.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auth_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(op =>
            {
                op.AddPolicy(
                    "_AllowSpecificOrigins", builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });
            services.Configure<Jwt>(Configuration.GetSection("Jwt"));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();


            
            // !! Add DBContext ===>
            // services.AddDbContext<AppDbContext>(option =>
            //     option
            //         .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            // );
            var sqlString = Configuration.GetConnectionString("SQLConnection");
            services.AddDbContext<AppDbContext>(option =>
                option.UseMySql(sqlString!, ServerVersion.Parse("8.0.30"))
            );

            //!! _ DependencyInjection _ ===>
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            
            // services.AddScoped<ICategoryServices, CategoryServices>();
            // services.AddScoped<IProductService, ProductService>();
            // services.AddScoped<ICartItemsService, CartItemsService>();
            // services.AddScoped<IOrdersService, OrdersService>();
            
            // services.AddSingleton<IUriService>(provider =>
            // {
            //     var accessor = provider.GetRequiredService<IHttpContextAccessor>();
            //     var request = accessor.HttpContext?.Request;
            //     var absoluteUri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent(), "/");
            //     return new UriService(absoluteUri);
            // });
            
            //AutoMapper
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(ApplicationUser));
            services.AddAutoMapper(typeof(AuthModel));
            services.AddAutoMapper(typeof(RegisterModel));
            services.AddAutoMapper(typeof(List<ApplicationUser>));
            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;
            });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>

                {
                    o.RequireHttpsMetadata = false;
                    //o.Authority = "https://localhost:5001";
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]!))
                    };
                });
            services.AddHttpContextAccessor();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppUserProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth_API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth_API v1"));
            }

            app.UseCors(AllowSpecificOrigins);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}