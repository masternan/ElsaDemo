using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services
    .AddElsaCore(elsa => elsa
        .AddHttpActivities()
        .AddConsoleActivities()
        .AddQuartzTemporalActivities()
        .UseEntityFrameworkPersistence(ef => ef.UseSqlite("Data Source=elsa.sqlite;Cache=Shared"))
    )
    .AddElsaApiEndpoints()
    .AddElsaSwagger(); 
builder.Services.AddRazorPages(); 

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition"))
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseHttpActivities()
   .UseRouting()
   .UseEndpoints(endpoints =>
   {
       endpoints.MapControllers();
       endpoints.MapFallbackToPage("/_Host");
   });

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elsa API v1");
});

app.MapRazorPages();

app.Run();

