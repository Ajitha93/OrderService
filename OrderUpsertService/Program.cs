using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderDBContext;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin() // or specify origins with .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // Ignore null values
        options.SerializerSettings.Formatting = Formatting.None; // Pretty print if needed
    }); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// Retrieve connection string from Key Vault
//var keyVaultUrl = "https://orderservicekeyvalut.vault.azure.net/"; // Your Key Vault URL
//var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
//KeyVaultSecret secret = client.GetSecret("orderconnectionstring");
//var connectionString = secret.Value;

//builder.Services.AddDbContext<RestaurantContext>(options =>
//               options.UseSqlServer(connectionString));

// Managed Identity start
//string sqlConnectionString = "Server=tcp:ordermgtsqlserver1.database.windows.net,1433;Database=orderscan";

string sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Use DefaultAzureCredential to authenticate via Managed Identity
var credential = new DefaultAzureCredential();
var token = await credential.GetTokenAsync(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));

// Use the token to authenticate to Azure SQL Database
var connection = new SqlConnection(sqlConnectionString);
connection.AccessToken = token.Token;

await connection.OpenAsync();

builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(connection));
// Managed Identity end


var app = builder.Build();

// Use CORS
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
