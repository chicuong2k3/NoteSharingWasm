using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using SharedKernel.Authentication;
using SharedKernel.Authentication.Requirements;
using SharedKernel.Contracts;
using SharingNote.Wasm;
using SharingNote.Wasm.ApiServices;
using SharingNote.Wasm.Auth;
using SharingNote.Wasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 1000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddMudMarkdownServices();

builder.Services.AddScoped<MarkdownService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiAddress"]!) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(PolicyConstants.CanManagePost, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new PostOwnerRequirement());
    });

    options.AddPolicy(PolicyConstants.CanManageTag, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new TagOwnerRequirement());
    });

    options.AddPolicy(PolicyConstants.CanManageComment, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new CommentOwnerRequirement());
    });
});

builder.Services.AddScoped<IAuthorizationHandler, PostOwnerAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TagOwnerAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CommentOwnerAuthorizationHandler>();
builder.Services.AddScoped<AuthorizationService>();

builder.Services.AddScoped<IReadPostService, PostService>();
builder.Services.AddScoped<IReadTagService, TagService>();
builder.Services.AddScoped<IReadCommentService, CommentService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICommentService, CommentService>();


await builder.Build().RunAsync();
