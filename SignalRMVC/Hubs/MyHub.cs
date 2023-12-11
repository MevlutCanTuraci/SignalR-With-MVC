using Microsoft.AspNetCore.SignalR;

namespace SignalRMVC.Hubs
{
    //Kaynak: https://learn.microsoft.com/tr-tr/aspnet/core/signalr/dotnet-client?view=aspnetcore-8.0&tabs=visual-studio
    //Grup kullanımı ile ilgili kaynak: https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups

    public class MyHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected. Connection Id: {Context.ConnectionId}");

            //Context.Abort(); Socket bağlantısını keser.
            //Context.ConnectionId Istek atan client'a ait bağlantı id değerini verir.
            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Disconnected. Connection Id: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }




        [HubMethodName("Test-Invoke-Method")]
        public string TestInvokeMethod(string userName, string message)
        {
            try
            {
                return $"{userName}: {message}";
            }
            catch (Exception e)
            {
                return $"Beklenmedik bir sistem hatası! Hata: {e}";
            }
        }


        [HubMethodName("Test-Send-Method")]
        public async Task TestSendMethod(string email, string message)
        {
            try
            {
                await Clients.Clients(Context.ConnectionId).SendAsync("SendMethodResponse", $"{email}: {message}");
            }
            catch (Exception e)
            {
                await Clients.Clients(Context.ConnectionId).SendAsync("SendMethodResponse", $"({nameof(TestSendMethod)}) Beklenmedik bir sistem hatası! Hata: {e}");
            }
        }


        /*
            HubMethodName attribute kullanılmadan da çalışır. Fakat ilgili fonksiyon adı neyse client tarafa o isim kullanılmalıdır.       
        */


        public async Task TestAttributeYok()
        {
            //Bu methodu client tarafında kullanabilmek için 'TestAttributeYok' isimi ile hub'a istek atılması gerekir.
            await Clients.Client(Context.ConnectionId).SendAsync("TestAttributeYokResponse", "Attribute olmayan method'dan response gönderildi.");
        }


        [HubMethodName("Butun-Bagli-Clientlara-Mesaj-Gonder")]
        public async Task ButunBaglantilaraMesajGonder()
        {
            //Sockete bağlı olan bütün client'lara mesaj gönderir.
            await Clients.All.SendAsync("AllClientResponse", "Bütün client'lara bu mesaj gider.");
        }


        [HubMethodName("Sadece-istek-gelen-clienta-mesaj-gonder")]
        public async Task SadeceIstekGelenClientaMesajGonder()
        {
            //Sadece istek atan client'a mesaj gönderir.
            await Clients.Clients(Context.ConnectionId).SendAsync("SadeceIstekAtanClientResponse", "Sadece isetk atan client'a bu mesaj gider.");
        }



        public Task JoinRoom(string roomName)
        {
            //Bağlanan client'i bir gruba ekler.
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            //Bağlanan client'i bir grupdan çıkartır.
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }


        [HubMethodName("Gruba-Ozel-Mesaj-Gonder")]
        public async Task GrubaOzelMesajGonderir(string name, string roomName)
        {
            //Sadece grup içerisinde ki client'lara mesaj gönderir.
            await Clients.Group(roomName).SendAsync(name + " odaya katıldı.");

        }



    }
}