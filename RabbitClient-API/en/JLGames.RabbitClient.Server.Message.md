# Namespace: JLGames.RabbitClient.Server.Message

Rabbit-Server message header, read/write, and request/response protocol. Little-endian by default. `IRabbitMessageReader` also implements Infra `IDataBufferReader` / `IDataBufferCopier` / `IByteBufferReader` / `IByteBufferCopier`, so `RabbitMessageReader` exposes typed read/copy helpers for primitives and arrays.

## IRabbitMessageHeader

Common header properties.

- `Extension`: Extension name
- `ProtoId`: Protocol id
- `ClientId`: Client identifier
- `ProtoUid`: Unique protocol id (`{Extension}:{ProtoId}`)

## IRabbitMessageContent

Extends `IRabbitMessageHeader` with raw payload accessors.

- `Content`: Message bytes
- `ContentString`: String representation of the payload

## IRabbitMessageReader

Extends `IRabbitMessageHeader` and the Buffer interfaces above.

- `Next`: Whether unread data remains
- `StartReadData()`: Reset the read position and read the header
- `ReadMessageTo(INetMessage message)` / `CopyMessageTo(INetMessage message)`: Read/copy an `INetMessage` (copy does not advance the index)
- `ReadDataTo(ref object data)` / `CopyDataTo(ref object data)`: Match primitives, arrays, or `INetMessage` by the runtime type of `data`
- `CopyRemains()`: Copy remaining bytes
- `SetMessageBytes(byte[] msg)`: Replace the buffer with raw bytes

## IRabbitMessageWriter

Extends `IRabbitMessageHeader`.

- `WriteHeader()`: Write the current header fields to the buffer
- `WriteHeader(string extension, string protoId, string uid)`: Set header fields and write (`uid` is the client id)
- `WriteMessage(INetMessage msg)`: Write an `INetMessage`
- `WriteData(object data)`: Write primitives and arrays, or an `INetMessage`
- `ToMessageBytes()`: Fully encoded message bytes

## IRabbitRequestMsg

Extends `IRabbitMessageWriter`.

- `SetProtoInfo(string extName, string protoId)`
- `SetClientId(string cid)`
- `StartWriteData()`: Write the header to start the request body
- `WriteRequestBase(object baseValue)`: Write a primitive (ignored if null)
- `WriteRequestMessage(INetMessage reqMsg)`: Write a message (ignored if null)

## IRabbitResponseMsg

Extends `IRabbitMessageReader` and `ICloneable<IRabbitResponseMsg>`.

- `RsCode`: Response status code

## RabbitMessageHeader

Public struct (not internal).

- Fields: `Extension`, `ProtoId`, `ClientId`
- `ProtoUid`: `{Extension}:{ProtoId}`
- `SetHeaderInfo(string extension, string protoId, string cid)`

## RabbitMessageWriter

Implements `IRabbitMessageWriter`.

- Properties: same as `IRabbitMessageHeader`
- Constructor: `RabbitMessageWriter(bool littleEndian = true)`
- Methods: same as the interface

## RabbitMessageReader

Implements `IRabbitMessageReader`.

- Properties: same as the interface (including `Next`)
- Constructor: `RabbitMessageReader(bool littleEndian = true)`
- Methods: same as the interface, plus typed read/copy helpers from the Buffer interfaces

## RabbitRequestMsg

Extends `RabbitMessageWriter`, implements `IRabbitRequestMsg`.

- Constructor: `RabbitRequestMsg(bool littleEndian = true)`
- Methods: same as `IRabbitRequestMsg`. `StartWriteData()` calls `WriteHeader()`.

## RabbitResponseMsg

Extends `RabbitMessageReader`, implements `IRabbitResponseMsg`.

- Property: `RsCode`
- Constructor: `RabbitResponseMsg(bool littleEndian = true)`
- `StartReadData()`: Read the header, then parse `RsCode`
- `SetMessageBytes(byte[] msg)`
- `Clone()`: Clone the current response, including unread buffer data
