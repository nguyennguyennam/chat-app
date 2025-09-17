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
                await ProcessRequestAsync(context);
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


        //Handle incoming messages, Open or Closed
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
                string receivedMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedMessage}");

                //Echo back the received message to the client
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
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