using System.Net.WebSockets;
using chat_service.infrastructure.Socket;
using chat_service.infrastructure.Utility.Database;
using chat_service.infrastructure.Migration;
using chat_service.infrastructure.Repository;
using chat_service.application.UseCase;
using chat_service.application.DTOs;
using chat_service.application.Interface;
using chat_service.domain.Repository;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Chat Console Demo - Cassandra + Migration + WebSocket");

        // 1. Connect Cassandra + run migration
        Console.WriteLine("⏳ Connecting to Cassandra...");
        var cassandraDb = new CassandraDb();
        Console.WriteLine("✅ Cassandra connected.");

        Console.WriteLine("⏳ Running migration...");
        var migration = new Message_Context(); // ensure table + index created
        Console.WriteLine("✅ Migration completed.");

        // 2. Setup repository + use case
        IChatRepository chatRepo = new MessageRepository(cassandraDb);
        IChatUseCase chatUseCase = new ChatUseCase(chatRepo);

        // 3. Start WebSocket server
        var server = new WebSocketServer();
        _ = Task.Run(() => server.StartAsync("localhost", 5000));
        await Task.Delay(1000);

        // 4. Create WebSocket clients
        var clientA = new ClientWebSocket();
        var clientB = new ClientWebSocket();

        await clientA.ConnectAsync(new Uri("ws://localhost:5000/"), CancellationToken.None);
        await clientB.ConnectAsync(new Uri("ws://localhost:5000/"), CancellationToken.None);

        var senderA = new SendMessage(clientA);
        var receiverA = new ReceiveMessage(clientA);

        var senderB = new SendMessage(clientB);
        var receiverB = new ReceiveMessage(clientB);

        _ = Task.Run(() => receiverA.ReceiveMessageFromServer());
        _ = Task.Run(() => receiverB.ReceiveMessageFromServer());

        Console.WriteLine("✅ Both users connected. Type messages to send...");

        Guid groupId = Guid.NewGuid(); // giả lập 1 group chat
        Guid userAId = Guid.NewGuid();
        Guid userBId = Guid.NewGuid();

        // 5. Chat loop
        while (true)
        {
            Console.Write("\n>>> Choose user (A/B) to send message or Q to quit: ");
            Console.Out.Flush();
            var choice = Console.ReadLine();

            if (string.Equals(choice, "Q", StringComparison.OrdinalIgnoreCase))
                break;

            Console.Write(">>> Enter message: ");
            var msgText = Console.ReadLine();

            Guid senderId = choice?.ToUpper() == "A" ? userAId : userBId;

            // Build request DTO
            var request = new SendMessageRequestDto
            {
                GroupId = groupId,
                SenderId = senderId,
                ContentText = msgText ?? string.Empty,
                ContentType = "Text",
                MimeType = "text/plain"
            };

            // Call use case (save Cassandra)
            var response = await chatUseCase.SendMessage(request);
            Console.WriteLine($"💾 Saved to Cassandra: {response.ContentText}");

            // Broadcast WebSocket
            if (choice?.ToUpper() == "A")
                await senderA.SendMessageToServer($"[User A]: {msgText}");
            else if (choice?.ToUpper() == "B")
                await senderB.SendMessageToServer($"[User B]: {msgText}");
        }

        Console.WriteLine("👋 Chat demo finished.");
    }
}
