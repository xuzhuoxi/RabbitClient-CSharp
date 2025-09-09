# Namespace: JLGames.RabbitClient

This namespace serves as the unified management entry point for Rabbit-Client-CSharp, responsible for coordinating connections and communication between Rabbit-Home and Rabbit-Server.

## Classes and Events

### RabbitClientManager
- Description: Unified client manager for Rabbit-Home queries and Rabbit-Server connections.
- Main Properties:
  - `HomeSettings`: Rabbit-Home configuration information.
  - `QueryInfo`: Current query information.
  - `HomeClient`: Rabbit-Home client.
  - `SocketServer`: Rabbit-Server communication server.
  - `SocketClient`: Rabbit-Server communication client.
- Main Methods:
  - `ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)`: Query and connect to server through Rabbit-Home.
  - `ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "Rabbit-Client")`: Query and connect to server through Rabbit-Home with random key support.
  - `ConnectThroughHome(QueryRouteInfo queryInfo)`: Connect using custom query information.

### RabbitClientManagerEvents
- Description: RabbitClientManager related event constants.
- Main Events:
  - `EventOnProgressHome`: Connection progress event (Rabbit-Home query phase).
  - `EventOnProgressServer`: Connection progress event (Rabbit-Server connection phase).
  - `EventOnConnectFinish`: Connection completion event.
- Event Data Types:
  - `ProgressEventData<T>`: Progress event data, containing `Suc` (whether successful) and `Data` (related data).
