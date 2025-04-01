using Delta;
using Hangfire;
using LeadwaycanteenApi;
using LeadwaycanteenApi.Hubs;
using LeadwaycanteenApi.Services.AuthService;
using LeadwaycanteenApi.Services.DbServices.AdminService;
using LeadwaycanteenApi.Services.DbServices.BranchService;
using LeadwaycanteenApi.Services.DbServices.EmployeeService;
using LeadwaycanteenApi.Services.DbServices.InventoryItemService;
using LeadwaycanteenApi.Services.DbServices.InventoryService;
using LeadwaycanteenApi.Services.DbServices.JwtManager;
using LeadwaycanteenApi.Services.DbServices.OrderService;
using LeadwaycanteenApi.Services.DbServices.ReviewService;
using LeadwaycanteenApi.Services.DbServices.VendorService;
using LeadwaycanteenApi.Services.EmailService;
using LeadwaycanteenApi.Services.JwtService;
using LeadwaycanteenApi.Services.OrderService;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped(_ => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.ConfigureJWTAuth(builder);
builder.Services.ConfigureHangFire(builder);
builder.Services.ConfigureCors();
builder.Services.AddLogging(c => c.AddConsole());
builder.Services.AddValidationUtility();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IJwtManager, JwtManager>();
builder.Services.AddTransient<IVendorService, VendorService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IDbOrderService, DbOrderService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IBranchService, BranchService>();
builder.Services.AddTransient<IInventoryItemService, InventoryItemService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IReviewService, ReviewService>();

builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen(opt =>
{
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LeadwaycanteenApiSwaggerAnnotation.xml")); 
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapScalarApiReference(opt =>
    {
        opt.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
        opt.Theme = ScalarTheme.Kepler;
    });
}

app.UseDelta();
app.UseWebSockets();
app.UseHangfireDashboard("/leadwaycanteenapi", new DashboardOptions
{
    DashboardTitle = "LeadwayCanteenApi Hangfire Dashboard",
    DarkModeEnabled = false,
    DisplayStorageConnectionString = false,
});
app.MapHub<OrdersHub>("/ordersHub");
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapControllers();

await app.RunAsync();
