

using KingICT_FlightSearchEngine.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});


//dependency injection
builder.Services.AddHttpClient("TravelAPI", client =>
{
    client.BaseAddress = new Uri("https://test.api.amadeus.com");

});
builder.Services.AddScoped<TravelAPI>();

builder.Services.AddControllersWithViews();


var app = builder.Build();



app.UseCors("AllowAll");
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}");
app.Run();
