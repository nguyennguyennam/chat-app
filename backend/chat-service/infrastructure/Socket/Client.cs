using System.Net.WebSockets;
using System.Net;

namespace chat_service.infrastructure.Socket;
/*
    This class implements the WebSocket client that connects to the chat server.
*/
public class Client
{
    private ClientWebSocket _Client;
    Client(ClientWebSocket client)
    {
        _Client = client;
    }
    public async Task ConnectServer(string ServerUri)
    {
        await _Client.ConnectAsync(new Uri(ServerUri), CancellationToken.None);
        Console.WriteLine("Connected to the server.");
    }
    
    public ClientWebSocket GetClient()
    {
        return _Client;
    }
}