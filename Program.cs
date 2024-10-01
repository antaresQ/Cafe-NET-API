using Cafe_NET_API.Data;
using Cafe_NET_API.Data.Interfaces;
using Cafe_NET_API.Helper;
using Cafe_NET_API.Services;
using Cafe_NET_API.Services.Interfaces;
using System.Text.Json.Serialization;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var AllowSpecificOrigins = "_allowSpecificOrigins";

var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}); ;

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {

        options.RedirectStatusCode = builder.Environment.IsDevelopment() ?
                                        Status307TemporaryRedirect : Status308PermanentRedirect;

        options.HttpsPort = builder.Configuration.GetValue<int>("https_port");

    });
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var sqliteConfig = builder.Configuration.GetSection("SQLiteConfig");
builder.Services.Configure<SQLiteConfig>(sqliteConfig);


var sqliteConfigVal = sqliteConfig.Get<SQLiteConfig>();
builder.Services.AddSingleton<DbContext>();


builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<ICafeRepository, CafeRepository>();
builder.Services.AddTransient<ICafeEmployeeRepository, CafeEmployeeRepository>();


builder.Services.AddSingleton<ICafeEmployeeService, CafeEmployeeService>();



var app = builder.Build();

// Setup DB and Tables
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DbContext>();
    await context.InitializeAsync();
}

app.UseCors(AllowSpecificOrigins);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//if (!builder.Environment.IsDevelopment())
//{
//    app.UseHttpsRedirection();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
