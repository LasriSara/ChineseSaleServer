
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using serverProjectFinal.BLL;
using serverProjectFinal.DAL;
using serverProjectFinal.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace serverProjectFinal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddAutoMapper(typeof(Startup));

            // Register your services and repositories
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDal, ProductDal>();
            services.AddScoped<IDonorService, DonorService>();
            services.AddScoped<IDonorDal, DonorDal>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDal, OrderDal>();
            services.AddScoped<ILotteryService, LotteryService>();
            services.AddScoped<ILotteryDal, LotteryDal>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerDal, CustomerDal>();

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
                        {
                            options.CustomSchemaIds(x => x.FullName);
                        });
            // Add Swagger documentation
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Add Entity Framework DbContext
            services.AddDbContext<PayingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PayingContext")));

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "development web site")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Add JWT authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    });

            // Add Session
            services.AddDistributedMemoryCache(); // Add storage for session data in-memory
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Create a cookie that is not accessible through JavaScript
                options.Cookie.IsEssential = true; // Cookie is essential for the application to function
            });

            services.AddMvc();
            services.AddControllers();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseSession();
            app.UseAuthorization();

            // Middleware for authentication
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Order"), OrderApp =>
            {
                app.UseMiddleware<AuthenticationMiddleware>();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
