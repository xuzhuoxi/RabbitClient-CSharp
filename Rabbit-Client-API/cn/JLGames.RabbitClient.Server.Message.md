# 命名空间：JLGames.RabbitClient.Server.Message

本命名空间包含与 Rabbit-Server 消息协议相关的接口与实现。

## 接口与类

### IRabbitMessage
- 描述：Rabbit 消息基础接口。
- 主要属性：
  - `Extension`：扩展名（Extension Name）。
  - `ProtoId`：协议Id（Proto Id）。
  - `ClientId`：消息客户端标识（Message client ID）。
  - `ProtoUid`：协议唯一Id（ProtoUID）。

### IRabbitMessageReader
- 描述：Rabbit 消息读取接口，继承自 IRabbitMessage、IDataBufferReader、IDataBufferCopier、IByteBufferReader、IByteBufferCopier。
- 主要属性：
  - `Next`：是否还有数据未读取（Is there any data left to read）。
- 主要方法：
  - `StartReadData()`：开始读取消息内容数据（Start to read message body data）。
  - `ReadMessageTo(INetMessage message)`：读取实现了INetMessage接口的对象数据。
  - `CopyMessageTo(INetMessage message)`：读取实现了INetMessage接口的对象数据，不移动读下标。
  - `ReadDataTo(ref object data)`：读取数据并保存到data中，根据data的数据类型进行匹配。
  - `CopyDataTo(ref object data)`：读取数据并保存到data中，不移动读下标。
  - `CopyRemains()`：复制剩余字节（Copy remain bytes）。
  - `SetMessageBytes(byte[] msg)`：使用字节数据更新当前对象的数据。

### IRabbitMessageWriter
- 描述：Rabbit 消息写入接口，继承自 IRabbitMessage。
- 主要方法：
  - `WriteHeader()`：重新设置消息表头，并写入到缓存（Set the message header, and write it to the buff）。
  - `WriteHeader(string extension, string protoId, string uid)`：重载，设置表头信息。
  - `WriteMessage(INetMessage msg)`：写入实现了INetMessage接口的对象数据。
  - `WriteData(object data)`：写入数据，支持INetMessage实现对象、基础数据类型及其数组。
  - `ToMessageBytes()`：读取消息字节数据。

### IRabbitRequestMsg
- 描述：Rabbit 请求消息接口，继承自 IRabbitMessageWriter。
- 主要方法：
  - `SetProtoInfo(string extName, string protoId)`：设置响应信息（Set response info）。
  - `SetClientId(string cid)`：设置客户端Id（Set client id）。
  - `StartWriteData()`：开始写入数据（Start write data）。
  - `WriteRequestBase(object baseValue)`：写入基数类型数据（Write base type data）。
  - `WriteRequestMessage(INetMessage reqMsg)`：写入消息数据（Write message data）。

### IRabbitResponseMsg
- 描述：Rabbit 响应消息接口，继承自 IRabbitMessageReader、ICloneable<IRabbitResponseMsg>。
- 主要属性：
  - `RsCode`：响应结构状态码（Response Result Code）。

### RabbitMessageWriter
- 描述：Rabbit 消息写入实现类，实现 IRabbitMessageWriter。
- 主要属性：
  - `Extension`：扩展名。
  - `ProtoId`：协议Id。
  - `ClientId`：客户端Id。
  - `ProtoUid`：协议唯一Id。
- 构造函数：
  - `RabbitMessageWriter(bool littleEndian = true)`：初始化，支持字节序设置。
- 主要方法：
  - `WriteHeader()`：写入表头。
  - `WriteHeader(string extension, string protoId, string cid)`：重载，设置表头信息。
  - `WriteMessage(INetMessage msg)`：写入INetMessage对象。
  - `WriteData(object data)`：写入数据。
  - `ToMessageBytes()`：获取消息字节数组。

### RabbitRequestMsg
- 描述：Rabbit 请求消息实现类，实现 IRabbitRequestMsg。
- 构造函数：
  - `RabbitRequestMsg(bool littleEndian = true)`：初始化，支持字节序设置。
- 主要方法：
  - `SetClientId(string cid)`：设置客户端Id。
  - `SetProtoInfo(string extName, string protoId)`：设置协议信息。
  - `StartWriteData()`：开始写入数据。
  - `WriteRequestBase(object baseValue)`：写入基础类型数据。
  - `WriteRequestMessage(INetMessage reqMsg)`：写入消息数据。

### RabbitMessageReader
- 描述：Rabbit 消息读取实现类，实现 IRabbitMessageReader。
- 主要属性：
  - `Extension`、`ProtoId`、`ClientId`、`ProtoUid`：同接口定义。
  - `Next`：是否还有数据未读取。
- 构造函数：
  - `RabbitMessageReader(bool littleEndian = true)`：初始化，支持字节序设置。
- 主要方法：
  - `StartReadData()`：开始读取数据。
  - `ReadMessageTo(INetMessage message)`、`CopyMessageTo(INetMessage message)`、`ReadDataTo(ref object data)`、`CopyDataTo(ref object data)`、`CopyRemains()`、`SetMessageBytes(byte[] msg)`等，详见接口定义。
  - 还包括各种基础类型和数组的读取、复制方法。

### RabbitResponseMsg
- 描述：Rabbit 响应消息实现类，实现 IRabbitResponseMsg。
- 主要属性：
  - `RsCode`：响应结构状态码。
- 构造函数：
  - `RabbitResponseMsg(bool littleEndian = true)`：初始化，支持字节序设置。
- 主要方法：
  - `StartReadData()`：重载，读取并解析RsCode。
  - `SetMessageBytes(byte[] msg)`：重载，设置原始消息字节。
  - `Clone()`：克隆当前响应消息对象。

### RabbitHeader（internal）
- 描述：Rabbit 消息头部结构体。
- 字段：
  - `Extension`：扩展名。
  - `ProtoId`：协议Id。
  - `ClientId`：客户端Id。
  - `ProtoUid`：协议唯一Id（`{Extension}:{ProtoId}`）。
- 方法：
  - `SetHeaderInfo(string extension, string protoId, string cid)`：设置表头信息。 