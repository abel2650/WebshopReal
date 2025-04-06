using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using ImaginaryShop.Model.Services;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using System.Diagnostics;

namespace ImaginaryShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();



            builder.Services.AddDistributedMemoryCache(); // Bruger hukommelsen til at lagre sessiondata
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // G�r cookien HttpOnly, s� den kun kan tilg�s af serveren (beskytter mod XSS-angreb)
                options.HttpOnly = HttpOnlyPolicy.Always;

                // Kun send cookies via sikre forbindelser (HTTPS)
                options.Secure = CookieSecurePolicy.Always;

                // SameSite politik (beskytter mod CSRF)
                options.MinimumSameSitePolicy = SameSiteMode.Strict;  // Brug Lax for at tillade nogle cross-origin anmodninger
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout for session
                options.Cookie.HttpOnly = true; // Forhindrer adgang til sessionen fra JavaScript
                options.Cookie.IsEssential = true; // S�rger for, at session virker uden cookie-consent
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Kr�ver HTTPS for session-cookies
            });

            builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        
            //S� cookies kan tilg�s i cshtml
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(options =>
      {

          options.Cookie.Name = "AuthCookie";  // Angiv navnet p� autentificeringscookien
          options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Kun HTTPS
          options.Cookie.SameSite = SameSiteMode.Lax;  // SameSite politik for autentificeringscookies
          options.Cookie.Expiration = null;  // Ingen udl�bsdato
          options.Cookie.MaxAge = null;   // Ingen max alder
          options.Cookie.IsEssential = true; // Cookien er n�dvendig for applikationen
      });




            var app = builder.Build();
            app.UseSession();  // Dette skal v�re f�r app.UseEndpoints()


            // Registrer IHttpContextAccessor


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();

            // Tilf�j autentificering og autorisation
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            Seed(app.Services);
            app.Run();
        }


        private static void Seed(IServiceProvider serviceProvider)
        {
            User u = new User();
            
            u.FullName = "Admin User";
            u.UserName = "admin123";
            u.Email = "admin@imaginaryshop.com";
            u.Role = User.UserRole.Admin;

            string pass = "Admin123456789"; // Strong password
            string hashedpassword = Argon2.Hash(pass);
            u.PasswordHash = hashedpassword;

            UserRepository ur = new UserRepository(serviceProvider.GetRequiredService<IConfiguration>());
            
            // Check if admin user already exists
            if (ur.GetUserByUserName(u.UserName) == null)
            {
                ur.CreateUser(u);
                Debug.WriteLine("Admin user created successfully");
            }
            else
            {
                Debug.WriteLine("Admin user already exists");
            }
        }
    }
}
