using LoanLedger.Application.Interfaces;
using LoanLedger.Infrastructure.Identity;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LoanLedger.Infrastructure.Repositories;
using LoanLedger.Application.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LoanLedger.Application.Contacts;
using LoanLedger.Application.Categories;
using LoanLedger.Application.Loans;
using System.Text.Json.Serialization;
using LoanLedger.Application.LoanTransactions;
using LoanLedger.Application.Workspaces;
using LoanLedger.Application.Items;
using LoanLedger.Application.Guarantors;
using LoanLedger.Application.Witnesses;
using LoanLedger.Application.LoanWitnesses;
using LoanLedger.Application.Collaterals;
using LoanLedger.Application.Attachments;
using LoanLedger.Infrastructure.Storage;
using LoanLedger.Application.Profile;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => char.ToLowerInvariant(x.Key[0]) + x.Key[1..],
                    x => x.Value!.Errors
                        .Select(e =>
                            string.IsNullOrWhiteSpace(e.ErrorMessage)
                                ? "Invalid value."
                                : e.ErrorMessage)
                        .ToArray());

            return new BadRequestObjectResult(new
            {
                success = false,
                message = "One or more validation errors occurred.",
                errors
            });
        };
    });

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["JwtSettings:Issuer"],

            ValidAudience =
                builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["JwtSettings:SecretKey"]!))
        };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<LoanLedgerDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("LoanLedgerConnection")));

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddScoped<IContactTrustRepository, ContactTrustRepository>();
builder.Services.AddScoped<IContactTrustService, ContactTrustService>();

builder.Services.AddScoped<IGuarantorRepository, GuarantorRepository>();
builder.Services.AddScoped<IGuarantorService, GuarantorService>();

builder.Services.AddScoped<IWitnessRepository, WitnessRepository>();
builder.Services.AddScoped<IWitnessService, WitnessService>();

builder.Services.AddScoped<ILoanWitnessRepository, LoanWitnessRepository>();
builder.Services.AddScoped<ILoanWitnessService, LoanWitnessService>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ILoanService, LoanService>();

builder.Services.AddScoped<ICollateralRepository, CollateralRepository>();
builder.Services.AddScoped<ICollateralService, CollateralService>();

builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddScoped<ILoanTransactionRepository, LoanTransactionRepository>();
builder.Services.AddScoped<LoanTransactionService>();
builder.Services.AddScoped<ILoanTransactionService, LoanTransactionService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();


// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
