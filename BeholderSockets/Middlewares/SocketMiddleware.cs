using BeholderSockets.Handlers;
using BeholderSockets.SocketsManager;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BeholderSockets.Middlewares
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        private SocketHandler handler { get; set; }

        public SocketMiddleware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            this.handler = handler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await handler.OnConnected(socket);

            await Receive(socket, async (result, buffer) => { 
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    await handler.Receive(socket, result, buffer);
                }
                else if(result.MessageType == WebSocketMessageType.Close)
                {
                    await handler.OnDisconnected(socket);
                }
            });
        }

        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> messageHandler)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandler(result, buffer);
            }
        }
    }
}
