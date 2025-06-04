using Microsoft.EntityFrameworkCore;
using SampleProject.Data;
using System.Text;
using System;
using Microsoft.Extensions.FileProviders;
using SampleProject.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Your React App URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddDbContext<EduSyncContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Debug);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();





app.UseAuthentication(); // <-- Add this
app.UseAuthorization();
app.UseStaticFiles();


app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();