using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PresupuestoMVC.Data;
using PresupuestoMVC.Repositories;
using PresupuestoMVC.Repository;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Text;

namespace PresupuestoMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Licencia de QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var builder = WebApplication.CreateBuilder(args);

            // Razor Runtime Compilation
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // Agregar servicios de localización
            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            // Configurar las culturas soportadas
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("es"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("es");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });

            var useProductionDatabase = builder.Configuration.GetValue<bool>("UseProductionDatabase");

            string connectionString = useProductionDatabase
                ? builder.Configuration.GetConnectionString("DefaultConnection")
                : builder.Configuration.GetConnectionString("DevelopmentConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("No se encontró la connection string configurada.");
            }

            //var connectionString =
            //    builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Registro del servicios
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IBudgetService, BudgetService>();
            builder.Services.AddScoped<IGastoService, GastoService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IDiaryService, DiaryService>();
            builder.Services.AddScoped<IDiaryRepository, DiaryRepository>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<ILocalidadService, LocalidadService>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<ILocalidadRepository, LocalidadRepository>();
            builder.Services.AddScoped<IArticuloService, ArticuloService>();
            builder.Services.AddScoped<IArticuloRepository, ArticuloRepository>();
            builder.Services.AddScoped<IPriceListService, PriceListService>();
            builder.Services.AddScoped<IPriceListRepository, PriceListRepository>();
            builder.Services.AddScoped<IProviderService, ProviderService>();
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
            builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            builder.Services.AddScoped<IArticulosPreciosService, ArticulosPreciosService>();
            builder.Services.AddScoped<IArticulosPreciosRepository, ArticulosPreciosRepository>();
            builder.Services.AddScoped<IPeriodoService, PeriodoService>();
            builder.Services.AddScoped<IPeriodRepository,PeriodRepository>();
            builder.Services.AddScoped<IProvinciaService, ProvinciaService>();
            builder.Services.AddScoped<IProvinciaRepository, ProvinciaRepository>();
            builder.Services.AddScoped<IFacturacionService, FacturacionService>();
            builder.Services.AddScoped<ISaleRepository, SaleRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // Configurar autenticación JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Validar el emisor del token
                    ValidateAudience = true, // Validar el destinatario del token
                    ValidateLifetime = true, // Validar la expiración del token
                    ValidateIssuerSigningKey = true, // Validar la firma del token

                    // Valores válidos para el token (deben coincidir con los usados al generar el token)
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],

                    // Clave secreta para verificar la firma (debe ser la misma que se usa para generar el token)
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
                };

                // Para leer JWT de cookies
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Middleware de localización
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseRequestLocalization(localizationOptions);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
