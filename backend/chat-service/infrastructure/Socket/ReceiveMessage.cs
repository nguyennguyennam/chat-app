using System.Net.WebSockets;
using System.Net;
namespace chat_service.infrastructure.Socket;

/*
    This file handles receive messages from server through web socket to the client. 
    It will read the whole message and send to the server.
    Method: ReceiveMessageFromServer
*/
public class ReceiveMessage
{
    private readonly ClientWebSocket _Client;
    public ReceiveMessage(ClientWebSocket client)
    {
        _Client = client;
    }


    public async Task ReceiveMessageFromServer ()
    {
        byte[] receiveBuffer = new byte[1024];
        try
        {
            while (_Client.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                using var ms = new MemoryStream();
                do
                {
                    result = await _Client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    ms.Write(receiveBuffer, 0, result.Count);
                }
                while (!result.EndOfMessage);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string receivedMessage = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        Console.WriteLine($"Received: {receivedMessage}");
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _Client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        Console.WriteLine("WebSocket connection closed.");
                    }
                }
        }
        catch (WebSocketException wse)
        {
            throw new InvalidOperationException("WebSocket error occurred: " + wse.Message);
        }
        
        // while (_Client.State == WebSocketState.Open)
        // {
        //     WebSocketReceiveResult result = await _Client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        //     if (result.MessageType == WebSocketMessageType.Text)
        //     {
        //         string receivedMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
        //         Console.WriteLine($"Received: {receivedMessage}");
        //     }
        //     else
        //     {
        //         Console.WriteLine("WebSocket is not connected.");
        //         throw new InvalidOperationException("WebSocket is not connected.");
        //     }
        // } 
    }
}