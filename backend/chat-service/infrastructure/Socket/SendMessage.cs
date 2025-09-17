using System.Net.WebSockets;
using System.Text;
using chat_service.infrastructure.Socket;
/*
    This file handles send messages from client through web socket to the server
*/

namespace chat_service.infrastructure.Socket;
public class SendMessage
{
    private readonly ClientWebSocket _Client;
    public SendMessage(ClientWebSocket client)
    {
        _Client = client;
    }
    public async Task SendMessageToServer(string message)
    {
        if (_Client.State == WebSocketState.Open)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _Client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"Sent: {message}");
        }
        else
        {
            Console.WriteLine("WebSocket is not connected.");
            throw new InvalidOperationException("WebSocket is not connected.");
        }
    }
}