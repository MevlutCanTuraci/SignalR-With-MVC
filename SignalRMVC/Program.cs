using MessagePack;
using SignalRMVC.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Projede SignalR etkinlestirme ve ayarlar�
builder.Services.AddSignalR(options =>
{   
    options.EnableDetailedErrors = true;
})
.AddMessagePackProtocol(options =>
{
    options.SerializerOptions = MessagePackSerializerOptions.Standard
    .WithSecurity(MessagePackSecurity.UntrustedData) //G�nderilen ve al�nan veriler i�in g�venlik ayar� yap�l�r.
    .WithCompression(MessagePackCompression.Lz4BlockArray); //G�nderilen ve al�nan yan�tlar�n s�k��t�r�lmas�n� sa�lar. 
});
//Projede SignalR etkinlestirme ve ayarlar�



//Ilgili cors ayar�
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


//Burada olu�turulan hublar rotalan�r.

//app.MapHub<Hub_S�n�f�>(<Rota>);

app.MapHub<MyHub>("/MyHub");


app.Run();
