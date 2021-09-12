using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Client.POC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string _token = "";
                string _anotherUser = "";

                Console.WriteLine("Select user: ");

                var usuario = Console.ReadLine();

                switch (usuario)
                {
                    case "1":
                        _token = "INFORM THE TOKEN";
                        _anotherUser = "ENTER USER ID";
                        break;
                    case "2":
                        _token = "INFORM THE TOKEN";
                        _anotherUser = "ENTER USER ID";
                        break;
                    default:
                        throw new Exception("Invalid option");
                }

                var connection = new HubConnectionBuilder()
                    .WithUrl("http://192.168.0.108/SignalR.AspNetCore.POC/messages?access_token=" + _token)
                    .Build();

                connection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                };

                //connection.On<string>("ReceiveMessage",
                //    (message) => Console.WriteLine($"{message}"));

                connection.On<string, string>("ReceiveMessage",
                    (user, message) => Console.WriteLine($"{user} : {message}"));

                await connection.StartAsync();

                Console.WriteLine("connected!");

                await connection.InvokeCoreAsync("JoinGroup", args: new[] { "1" });
                Console.WriteLine("Add to group '1'! ");

                string message = "";
                do
                {
                    Console.WriteLine("Write message or (q)uit");

                    message = Console.ReadLine();

                    await connection.InvokeCoreAsync("SendMessageToUser", args: new[] { _anotherUser, message });
                }
                while (message != "q");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

        }
    }
}
