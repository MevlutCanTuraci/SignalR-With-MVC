using MessagePack;
using SignalRMVC.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Projede SignalR etkinlestirme ve ayarlarý
builder.Services.AddSignalR(options =>
{   
    options.EnableDetailedErrors = true;
})
.AddMessagePackProtocol(options =>
{
    options.SerializerOptions = MessagePackSerializerOptions.Standard
    .WithSecurity(MessagePackSecurity.UntrustedData) //Gönderilen ve alýnan veriler için güvenlik ayarý yapýlýr.
    .WithCompression(MessagePackCompression.Lz4BlockArray); //Gönderilen ve alýnan yanýtlarýn sýkýþtýrýlmasýný saðlar. 
});
//Projede SignalR etkinlestirme ve ayarlarý



//Ilgili cors ayarý
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//Burada oluþturulan hublar rotalanýr.

//app.MapHub<Hub_Sýnýfý>(<Rota>);

app.MapHub<MyHub>("/MyHub");


app.Run();
