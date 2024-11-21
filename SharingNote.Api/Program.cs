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
using SharingNote.Api.GraphQL;
using SharingNote.Api.Application.Features.Posts;
using Akismet;
using SharingNote.Api.Hubs;
using SharingNote.Api.Infrastructure.Services;
using SharingNote.Api.Application.Services;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection.Extensions;

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


#region GraphQL

builder.AddGraphQL()
    .AddType<PostDto>()
    .AddSharingNoteTypes()
    .ModifyOptions(options =>
    {
        options.StripLeadingIFromInterface = true;
        options.EnableOneOf = true;
    })
    .AddProjections()
    .AddFiltering(configs =>
    {
        configs.AddDefaults();
    })
    .AddSorting()
    .AddPagingArguments();


#endregion

#region third party services

builder.Services.AddSingleton(
        new AkismetClient(
        builder.Configuration["Akismet:ApiKey"]!,
        new Uri(builder.Configuration["ApiBaseUrl"]!),
        "notesharing")
    );

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Brevo"));
builder.Services.AddSingleton<IEmailSendingService, BrevoSendingEmailService>();
builder.Services.AddSingleton<IOtpService, OtpService>();

#endregion

#region Caching
builder.Services.TryAddSingleton<ICacheService, CacheService>();

try
{
    var cacheConnectionString = builder.Configuration.GetConnectionString("Redis")!;
    var connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString);
    builder.Services.TryAddSingleton(connectionMultiplexer);
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.ConnectionMultiplexerFactory = ()
        => Task.FromResult<IConnectionMultiplexer>(connectionMultiplexer);
    });

    Console.WriteLine("Connected to Redis");
}
catch
{
    builder.Services.AddDistributedMemoryCache();
}
#endregion

#region SignalR

builder.Services.AddSignalR();

#endregion


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

app.MapHub<ViewHub>("/hubs/view");
app.MapHub<InteractionHub>("/hubs/interaction");

app.MapGraphQL();
app.RunWithGraphQLCommands(args);


app.Run();

