using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra
{
    internal class SignalRService
    {
        private HubConnection _hubConnection;

        public async Task ConnectAsync(string url)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            // Handle receiving messages from the hub
            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
                // Update the UI in .NET MAUI here (e.g., adding messages to a list)
            });

            // Handle live note updates
            _hubConnection.On<string>("ReceiveNoteUpdate", (noteContent) =>
            {
                Console.WriteLine($"Updated Note: {noteContent}");
                // Update the notes UI here
            });

            await _hubConnection.StartAsync();
        }

        public async Task SendMessageAsync(string user, string message)
        {
            await _hubConnection.InvokeAsync("SendMessage", user, message);
        }

        public async Task UpdateNoteAsync(string noteContent)
        {
            await _hubConnection.InvokeAsync("UpdateNote", noteContent);
        }
    }
}
