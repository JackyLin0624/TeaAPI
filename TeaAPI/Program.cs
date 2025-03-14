using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using TeaAPI.AutoMappers;
using TeaAPI.Middlewares;
using TeaAPI.Models.Auths;
using TeaAPI.Repositories.Accounts;
using TeaAPI.Repositories.Accounts.Interfaces;
using TeaAPI.Repositories.Orders;
using TeaAPI.Repositories.Orders.Interfaces;
using TeaAPI.Repositories.Products;
using TeaAPI.Repositories.Products.Interfaces;
using TeaAPI.Services.Account;
using TeaAPI.Services.Account.Interfaces;
using TeaAPI.Services.Orders;
using TeaAPI.Services.Orders.Interfaces;
using TeaAPI.Services.Products;
using TeaAPI.Services.Products.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LoginRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown", 
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3, 
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            })
    );
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(
            ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }
        );
    });
});


builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", policy =>
    {
        policy
        .WithOrigins("http://localhost:4200") 
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
//AutoMappers
builder.Services.AddAutoMapper(typeof(TeaMappingProfile));


// Add services to the container.
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("JWTConfig").Get<JWTConfig>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageOrders", policy => policy.RequireClaim("Permission", "ORDER_MANAGE"));
    options.AddPolicy("CanManageProducts", policy => policy.RequireClaim("Permission", "PRODUCT_MANAGE"));
    options.AddPolicy("CanManageAccounts", policy => policy.RequireClaim("Permission", "ACCOUNT_MANAGE"));
    options.AddPolicy("CanManageProductsOrOrder", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("Permission", "ORDER_MANAGE") ||
            context.User.HasClaim("Permission", "PRODUCT_MANAGE")
        ));
});


builder.Services.AddScoped<IPermissionRepository, PermissionRepository>(x => new PermissionRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<IRolePermissionRepository, RolePemissionRepository>(x => new RolePemissionRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IRoleRepository, RoleRepository>(x => new RoleRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IRoleService, RoleService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IRefreshTokenCacheService, RefreshTokenMemoryCacheService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>(x => new ProductCategoryRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

builder.Services.AddScoped<IVariantTypeRepository, VariantTypeRepository>(x => new VariantTypeRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IVariantValueRepository, VariantValueRepository>(x => new VariantValueRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IProductVariantOptionRepository, ProductVariantOptionRepository>(x => new ProductVariantOptionRepository(x.GetRequiredService<IConfiguration>()));

builder.Services.AddScoped<IVariantService, VariantService>();


builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>(x => new ProductSizeRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IProductRepository, ProductRepository>(x => new ProductRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IOrderItemOptionRepository, OrderItemOptionRepository>(x => new OrderItemOptionRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>(x => new OrderItemRepository(x.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IOrderRepository, OrderRepository>(x => new OrderRepository(x.GetRequiredService<IConfiguration>()));

builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TeaAPI", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "please input JWTToken，format：Bearer {your_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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
            new string[] {}
        }
    });
});

var app = builder.Build();
app.UseRateLimiter();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
