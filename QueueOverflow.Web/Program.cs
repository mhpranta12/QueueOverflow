using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using QueueOverflow.Web;
using QueueOverflow.Infrastructure;
using QueueOverflow.Infrastructure.Extensions;
using QueueOverflow.Application;
using QueueOverflow.Infrastructure.Email;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration));

try
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new ApplicationModule());
        containerBuilder.RegisterModule(new InfrastructureModule(connectionString,
            migrationAssembly));
        containerBuilder.RegisterModule(new WebModule());
    });


    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString,
        (m) => m.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddIdentity();
    builder.Services.AddControllersWithViews();
    builder.Services.AddCookieAuthentication();
    // Policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("Admin");
        });
        options.AddPolicy("EmailConfirmationPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Email Confirmed", "true");
        });
        options.AddPolicy("ProfileViewingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("View Profile", "true");
        });
        options.AddPolicy("ProfileEditingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Edit Profile", "true");
        });
        options.AddPolicy("QuestionPostingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Post Question", "true");
            policy.RequireClaim("Email Confirmed", "true");
        });
        options.AddPolicy("QuestionSearchingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Search Question", "true");
        });
        options.AddPolicy("QuestionViewingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("View Questions", "true");
			policy.RequireClaim("Email Confirmed", "true");

		});
        options.AddPolicy("QuestionReplyingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Reply On Question", "true");
            policy.RequireClaim("Email Confirmed", "true");
        });
        options.AddPolicy("QuestionCommentingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Comment On Question", "true");
            policy.RequireClaim("Email Confirmed", "true");

        });
        options.AddPolicy("QuestionVotingPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("Vote Question", "true");
            policy.RequireClaim("Email Confirmed", "true");

        });
    });
    builder.Services.ConfigureApplicationCookie(
        options =>
        {
            options.AccessDeniedPath = "/Account/AccessDenied";
        });
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(10);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));
    
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection()
        .UseStaticFiles()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseSession();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();

    Log.Information("Application Starting...");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application.");
}
finally
{
    Log.CloseAndFlush();
}

