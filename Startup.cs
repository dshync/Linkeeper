using AutoMapper;
using Linkeeper.Data;
using Linkeeper.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Linkeeper
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
			services.AddDbContext<LinkeeperContext>(options => options.UseMySql
				(Configuration.GetConnectionString("LinkeeperConnection"),
				new MySqlServerVersion(new Version(5, 7))));

			services.AddDefaultIdentity<IdentityUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
			})
				.AddEntityFrameworkStores<LinkeeperContext>();

			services.AddControllersWithViews();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			//services.AddSingleton<ILinkeeperRepo, MockLinkeeperRepo>();
			services.AddScoped<ILinkeeperRepo, MySqlLinkeeperRepo>();

			/*JwtSettings jwt = new JwtSettings();
			Configuration.Bind(nameof(jwt), jwt);*/
			JwtSettings jwt = new JwtSettings { Secret = Configuration["JwtSettings:Secret"] };
			services.AddSingleton<JwtSettings>(jwt);

			services.AddAuthentication(options =>
			{
				//options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireExpirationTime = false,
					ValidateLifetime = true
				};
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
