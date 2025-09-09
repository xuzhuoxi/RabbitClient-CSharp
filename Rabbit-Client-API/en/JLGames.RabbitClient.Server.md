# Namespace: JLGames.RabbitClient.Server

This namespace contains core interfaces and implementations related to Rabbit-Server connections, events, default configurations, etc.

## Classes and Interfaces

### RabbitSocketClient
- Description: Rabbit-Server client, responsible for communicating with the server.
- Main Properties:
  - `Connected`: Whether connected.
  - `EventDispatcher`: Event dispatcher.
- Constructor:
  - `RabbitSocketClient(RabbitSocketServer socketServer)`: Initialize client.
- Main Methods:
  - `SetThreadContext(FixedThreadContext context)`: Set thread context.
  - `SetSymmetricCipher(ICipher cipher)`: Set symmetric key.
  - `StartReceiving()`: Start receiving messages.
  - `StopReceiving()`: Stop receiving messages.
  - `SendMessage(IRabbitMessageWriter msg)`: Send message.
  - `Dispose()`: Release resources.

### RabbitSocketServer
- Description: Rabbit-Server server side, responsible for communicating with clients.
- Main Properties:
  - `SocketServer`: Underlying Socket client.
  - `Connecting`: Whether connecting or disconnecting.
  - `Connected`: Whether connected.
- Constructor:
  - `RabbitSocketServer()`: Initialize server.
- Main Methods:
  - `SetThreadContext(FixedThreadContext context)`: Set thread context.
  - `ConnectServer(QueryRouteBackInfo serverInfo)`: Connect to server.
  - `DisconnectServer()`: Disconnect from server.

### RabbitServerDefaults
- Description: Rabbit-Server related constants and default configuration.
- Main Properties/Methods:
  - `LittleEndian`: Byte order setting.
  - `ApmMode`: API connection mode.
  - `SetLittleEndian(bool)`: Set byte order.
  - `SetConnectApiMode(bool)`: Set API connection mode.

## Events

### RabbitSocketServerEvents
- Event Constants:
  - `EventOnConnectionOpenSuc`: Connection success event (event data: SocketEvents.SocketConnEventInfo).
  - `EventOnConnectionOpenFail`: Connection failure event (event data: SocketEvents.SocketConnEventInfo).
  - `EventOnConnectionClose`: Close connection result event (event data: SocketEvents.SocketConnEventInfo).
  - `EventOnServerMessage`: Data reception processing completed (event data: byte[] message).

### RabbitSocketClientEvents
- Event Constants:
  - `EventOnClientMessage`: Data reception processing completed (event data: IRabbitResponseMsg).
