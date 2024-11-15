using Serilog;
using SharingNote.Api.Application.Behaviours;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SharedKernel.Authentication.Requirements;
using SharedKernel.Authentication;
using SharingNote.Api;
using SharedKernel.Contracts;
using SharingNote.Api.ContractsImplementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Logging
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
#endregion

#region EF & Authentication & Authorization
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var secret = builder.Configuration["JwtAuthSettings:Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtAuthSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtAuthSettings:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });

builder.Services.AddAuthorization(options =>
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

builder.Services.AddScoped<IReadCommentService, CommentService>();
builder.Services.AddScoped<IReadPostService, PostService>();
builder.Services.AddScoped<IReadTagService, TagService>();


#endregion

#region MediatR

List<Assembly> assemblies = [Assembly.GetExecutingAssembly()];

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assemblies.ToArray());

    config.AddOpenBehavior(typeof(ExceptionHandlingBehaviour<,>));
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(RequestLoggingBehaviour<,>));
});

builder.Services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

#endregion

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    { // .WithOrigins(builder.Configuration["BaseUrl"]!)
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.SeedDatabase();

app.UseHttpsRedirection();
app.UseCors();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
