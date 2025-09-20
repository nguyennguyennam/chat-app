    /*
        This file connects to the Cassandra database via Docker.
    */

    using Cassandra;
    using System.Security.Authentication;
    using Cassandra.Mapping;

    namespace chat_service.infrastructure.Utility.Database;

   
    public class CassandraDb
    {
        
        private readonly Cassandra.ISession _session;
        private readonly IMapper _mapper;
        private static readonly string _keyspace = "chat_app";
        private static readonly string _table = "messages";
        private static readonly string _contactPoint = "cassandra"; // Docker service name
        private static readonly int _port = 9042;
        private static readonly string _username = "groupchat_cassandra";
        private static readonly string password = "hello_world";


        public CassandraDb()
    {
            var SSLOptions = new SSLOptions(
            SslProtocols.Tls12,
            true,
            (sender, certificate, chain, sslPolicyErrors) => true // Accept all certificates (for development purposes only)
        );
            var cluster = Cluster.Builder()
                .AddContactPoint(_contactPoint)
                .WithPort(_port)
                .WithCredentials(_username, password)
                .WithSSL(SSLOptions)
                .Build();
            // Initialize the session and mapper
            _session = cluster.Connect(_keyspace);


            //Make sure the keyspace exists
            _session.Execute($@"
            CREATE KEYSPACE IF NOT EXISTS {_keyspace}
            WITH REPLICATION = {{ 'class': 'SimpleStrategy', 'replication_factor': 3 }};");

            //Switch to the keyspace
            _session.ChangeKeyspace(_keyspace);

            //Gain new mapper
            _mapper = new Mapper(_session);
        }

        public Cassandra.ISession GetSession()
        {
            return _session;
        }
        public IMapper GetMapper()
        {
            return _mapper;
        }
    }
