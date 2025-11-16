
using Hangfire;
using HangfireBasicAuthenticationFilter;
using NileFood.API;
using NileFood.Application;
using NileFood.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services
    .AddApiExtensions(builder.Configuration)
    .AddApplicationExtensions(builder.Configuration)
    .AddInfrastructureExtensions(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();




app.UseHangfireDashboard("/jobs", new DashboardOptions()
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            User  = app.Configuration.GetValue<string>("HangfireSettings:Username"),
            Pass  = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
    ],
    DashboardTitle = "NileMind Dashboard",
    // IsReadOnlyFunc = (DashboardContext context) => true

});
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
