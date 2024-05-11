


using Components.Pages;
using Implementations.Users;
using Main.Comps;
using Microsoft.JSInterop;
using Org.Apps;
using Org.Apps.Email;
using Org.Apps.Users;
using Org.Impl;
using Org.Impl.Email;
using Org.Storages.UserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<IOrgTypeService, OrgTypeService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserStorage, UserStorage>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();
builder.Services.AddScoped<IUserRolesStorage, UserRolesStorage>();
builder.Services.AddControllers();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies",options =>
{   
    options.ExpireTimeSpan=TimeSpan.FromHours(5);
    options.LoginPath = "/Login";
    options.Cookie.Name = "Organigram";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode().AddAdditionalAssemblies(typeof(LoginPage).Assembly);

app.Run();