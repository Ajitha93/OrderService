using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderDBContext;


var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("http://0.0.0.0:81");

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


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
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Apply the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
