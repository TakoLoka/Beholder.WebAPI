using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BeholderSockets.SocketsManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _connections.FirstOrDefault(connection => connection.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllConnections()
        {
            return _connections;
        }

        public string GetId(WebSocket socket)
        {
            return _connections.FirstOrDefault(connection => connection.Value.Equals(socket)).Key;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _connections.TryRemove(id, out var socket);

            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Socket Connection Closed", CancellationToken.None);
        }

        public void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionId(), socket);
        }
        private string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
