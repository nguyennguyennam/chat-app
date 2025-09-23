using System.Net.WebSockets;
using System.Net;
namespace chat_service.infrastructure.Socket;

/*
    This class establishes a Websocket server to handle real-time communication for the chat application
*/

public class WebSocketServer
{
    //Start the Websocket Server
    public async Task StartAsync(string IpAddress, int port)
    {
        HttpListener listener = new HttpListener();
        Console.WriteLine($"Starting Websocket server on ws://{IpAddress}:{port}/");
        // Add the prefix to the listener
        listener.Prefixes.Add($"http://{IpAddress}:{port}/");
        listener.Start();

        Console.WriteLine("Server started. Waiting for connections...");

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                // Do not await here — process each connection concurrently so the listener can accept more clients
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await ProcessRequestAsync(context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WebSocket connection processing error: {ex}");
                    }
                });
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }
    
    /*
        Process each WebSocket Request
    */
    public async Task ProcessRequestAsync (HttpListenerContext context)
    {
        HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
        WebSocket webSocket = wsContext.WebSocket;
        Console.WriteLine("WebSocket connection established.");


        //Handle incoming messages (accumulate frames into MemoryStream and use the full message bytes)
        byte[] buffer = new byte[1024];
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result;
            using var ms = new MemoryStream();
            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                ms.Write(buffer, 0, result.Count);
            }
            while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var messageBytes = ms.ToArray();
                string receivedMessage = System.Text.Encoding.UTF8.GetString(messageBytes);
                Console.WriteLine($"Received: {receivedMessage}");

                // Echo back the received message to the client using the full message bytes
                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            //Handle Close message
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                Console.WriteLine("WebSocket connection closed.");
            }
        }
    }
}