# Namespace: JLGames.RabbitClient.Server.Message

This namespace contains interfaces and implementations related to Rabbit-Server message protocols.

## Interfaces and Classes

### IRabbitMessage
- Description: Rabbit message base interface.
- Main Properties:
  - `Extension`: Extension Name.
  - `ProtoId`: Proto Id.
  - `ClientId`: Message client ID.
  - `ProtoUid`: ProtoUID.

### IRabbitMessageReader
- Description: Rabbit message reading interface, inherits from IRabbitMessage, IDataBufferReader, IDataBufferCopier, IByteBufferReader, IByteBufferCopier.
- Main Properties:
  - `Next`: Is there any data left to read.
- Main Methods:
  - `StartReadData()`: Start to read message body data.
  - `ReadMessageTo(INetMessage message)`: Read object data that implements INetMessage interface.
  - `CopyMessageTo(INetMessage message)`: Read object data that implements INetMessage interface, without moving read index.
  - `ReadDataTo(ref object data)`: Read data and save to data, matching based on data's data type.
  - `CopyDataTo(ref object data)`: Read data and save to data, without moving read index.
  - `CopyRemains()`: Copy remain bytes.
  - `SetMessageBytes(byte[] msg)`: Update current object's data using byte data.

### IRabbitMessageWriter
- Description: Rabbit message writing interface, inherits from IRabbitMessage.
- Main Methods:
  - `WriteHeader()`: Set the message header, and write it to the buff.
  - `WriteHeader(string extension, string protoId, string uid)`: Overload, set header information.
  - `WriteMessage(INetMessage msg)`: Write object data that implements INetMessage interface.
  - `WriteData(object data)`: Write data, supports INetMessage implementation objects, basic data types and their arrays.
  - `ToMessageBytes()`: Read message byte data.

### IRabbitRequestMsg
- Description: Rabbit request message interface, inherits from IRabbitMessageWriter.
- Main Methods:
  - `SetProtoInfo(string extName, string protoId)`: Set response info.
  - `SetClientId(string cid)`: Set client id.
  - `StartWriteData()`: Start write data.
  - `WriteRequestBase(object baseValue)`: Write base type data.
  - `WriteRequestMessage(INetMessage reqMsg)`: Write message data.

### IRabbitResponseMsg
- Description: Rabbit response message interface, inherits from IRabbitMessageReader, ICloneable<IRabbitResponseMsg>.
- Main Properties:
  - `RsCode`: Response Result Code.

### RabbitMessageWriter
- Description: Rabbit message writing implementation class, implements IRabbitMessageWriter.
- Main Properties:
  - `Extension`: Extension name.
  - `ProtoId`: Proto Id.
  - `ClientId`: Client Id.
  - `ProtoUid`: Proto unique Id.
- Constructor:
  - `RabbitMessageWriter(bool littleEndian = true)`: Initialize, supports byte order setting.
- Main Methods:
  - `WriteHeader()`: Write header.
  - `WriteHeader(string extension, string protoId, string cid)`: Overload, set header information.
  - `WriteMessage(INetMessage msg)`: Write INetMessage object.
  - `WriteData(object data)`: Write data.
  - `ToMessageBytes()`: Get message byte array.

### RabbitRequestMsg
- Description: Rabbit request message implementation class, implements IRabbitRequestMsg.
- Constructor:
  - `RabbitRequestMsg(bool littleEndian = true)`: Initialize, supports byte order setting.
- Main Methods:
  - `SetClientId(string cid)`: Set client Id.
  - `SetProtoInfo(string extName, string protoId)`: Set protocol information.
  - `StartWriteData()`: Start write data.
  - `WriteRequestBase(object baseValue)`: Write basic type data.
  - `WriteRequestMessage(INetMessage reqMsg)`: Write message data.

### RabbitMessageReader
- Description: Rabbit message reading implementation class, implements IRabbitMessageReader.
- Main Properties:
  - `Extension`, `ProtoId`, `ClientId`, `ProtoUid`: Same as interface definition.
  - `Next`: Is there any data left to read.
- Constructor:
  - `RabbitMessageReader(bool littleEndian = true)`: Initialize, supports byte order setting.
- Main Methods:
  - `StartReadData()`: Start read data.
  - `ReadMessageTo(INetMessage message)`, `CopyMessageTo(INetMessage message)`, `ReadDataTo(ref object data)`, `CopyDataTo(ref object data)`, `CopyRemains()`, `SetMessageBytes(byte[] msg)`, etc., see interface definition for details.
  - Also includes various basic type and array reading, copying methods.

### RabbitResponseMsg
- Description: Rabbit response message implementation class, implements IRabbitResponseMsg.
- Main Properties:
  - `RsCode`: Response structure status code.
- Constructor:
  - `RabbitResponseMsg(bool littleEndian = true)`: Initialize, supports byte order setting.
- Main Methods:
  - `StartReadData()`: Overload, read and parse RsCode.
  - `SetMessageBytes(byte[] msg)`: Overload, set original message bytes.
  - `Clone()`: Clone current response message object.

### RabbitHeader (internal)
- Description: Rabbit message header structure.
- Fields:
  - `Extension`: Extension name.
  - `ProtoId`: Proto Id.
  - `ClientId`: Client Id.
  - `ProtoUid`: Proto unique Id (`{Extension}:{ProtoId}`).
- Methods:
  - `SetHeaderInfo(string extension, string protoId, string cid)`: Set header information.
