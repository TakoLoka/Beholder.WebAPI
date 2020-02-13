using BeholderSockets.SocketsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeholderSockets.Handlers
{
    public abstract class SocketHandler
    {
        public ConnectionManager Connections { get; set; }

        public SocketHandler(ConnectionManager connectionManager)
        {
            Connections = connectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { Connections.AddSocket(socket); });
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Task.Run(async () => await Connections.RemoveSocketAsync(Connections.GetId(socket)));
        }

        public virtual async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await SendAsyncFromSocket(socket, message);
        }

        public virtual async Task SendMessage(string id, string message)
        {
            var socket = Connections.GetSocketById(id);

            if (socket.State != WebSocketState.Open)
                return;

            await SendAsyncFromSocket(socket, message);
        }

        public virtual async Task SendMessageToAll(string message)
        {
            foreach(var connection in Connections.GetAllConnections())
            {
                await SendMessage(connection.Value, message);
            }
        }

        private async Task SendAsyncFromSocket(WebSocket socket, string message)
        {
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
