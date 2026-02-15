/// <summary>
/// Task Project Management System - Application Entry Point
/// 
/// This file configures:
/// - Dependency Injection services
/// - Razor Components (Blazor Web App)
/// - API Controllers
/// - Swagger / OpenAPI documentation
/// - HTTP request pipeline middleware
/// 
/// Swagger UI is enabled only in Development environment.
/// </summary>

using Microsoft.OpenApi;
using Task_Project_Management_System.Client.Pages;
using Task_Project_Management_System.Components;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registers application services into the Dependency Injection container.
/// </summary>

/// <summary>
/// Adds Razor Components with Interactive WebAssembly support.
/// Enables hybrid rendering (Server + WebAssembly).
/// </summary>
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

/// <summary>
/// Adds MVC Controllers to support REST API endpoints.
/// Required for exposing API routes like /api/tasks.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Enables API endpoint discovery required for Swagger.
/// </summary>
builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Configures Swagger/OpenAPI documentation generation.
/// Defines API metadata such as Title, Version, and Description.
/// </summary>
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Task Project Management System API",
        Description = "API documentation for the Task Project Management System application."
    });
});

var app = builder.Build();

/// <summary>
/// Configures the HTTP request pipeline.
/// Middleware order is important.
/// </summary>

/// <summary>
/// Development Environment Configuration.
/// Enables debugging and Swagger UI.
/// </summary>
if (app.Environment.IsDevelopment())
{
    /// <summary>
    /// Enables WebAssembly debugging support.
    /// </summary>
    app.UseWebAssemblyDebugging();

    /// <summary>
    /// Enables Swagger JSON endpoint generation.
    /// </summary>
    app.UseSwagger();

    /// <summary>
    /// Enables Swagger UI for API testing.
    /// Accessible at: https://localhost:{port}/api-docs
    /// </summary>
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Task Project Management System API v1"
        );

        options.RoutePrefix = "api-docs";
    });
}
else
{
    /// <summary>
    /// Configures global exception handling for production.
    /// </summary>
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    /// <summary>
    /// Enables HTTP Strict Transport Security (HSTS).
    /// </summary>
    app.UseHsts();
}

/// <summary>
/// Handles 404 and other status code pages.
/// Redirects to custom not-found page.
/// </summary>
app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true
);

/// <summary>
/// Redirects HTTP requests to HTTPS.
/// </summary>
app.UseHttpsRedirection();

/// <summary>
/// Serves static files from wwwroot.
/// </summary>
app.UseStaticFiles();

/// <summary>
/// Enables CSRF protection.
/// </summary>
app.UseAntiforgery();

/// <summary>
/// Maps API controller endpoints.
/// Example route: /api/tasks
/// </summary>
app.MapControllers();

/// <summary>
/// Maps static assets required by Razor Components.
/// </summary>
app.MapStaticAssets();

/// <summary>
/// Maps Razor Components and enables interactive WebAssembly rendering.
/// </summary>
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(Task_Project_Management_System.Client._Imports).Assembly
    );

/// <summary>
/// Starts the web application.
/// </summary>
app.Run();
