using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GameStore.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5270") });

await builder.Build().RunAsync();


// Alternate app initialization for hot reloading with SignalR
// using Microsoft.AspNetCore.Components.Web;
// using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
// using Microsoft.AspNetCore.SignalR.Client; // Import SignalR client namespace
// using GameStore.Client;

// var builder = WebAssemblyHostBuilder.CreateDefault(args);

// builder.RootComponents.Add<App>("#app");
// builder.RootComponents.Add<HeadOutlet>("head::after");

// // Add HttpClient with base address
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// // Add SignalR client
// builder.Services.AddSingleton<HubConnection>(sp =>
// {
//     var hubConnection = new HubConnectionBuilder()
//         .WithUrl(builder.HostEnvironment.BaseAddress + "chatHub") // Adjust the URL based on your SignalR Hub endpoint
//         .Build();

//     // Start the connection
//     hubConnection.StartAsync().ContinueWith(task =>
//     {
//         if (task.IsFaulted)
//         {
//             Console.WriteLine("Error starting SignalR connection: " + task.Exception!.GetBaseException());
//         }
//         else
//         {
//             Console.WriteLine("SignalR connection started.");
//         }
//     });

//     return hubConnection;
// });

// await builder.Build().RunAsync();
