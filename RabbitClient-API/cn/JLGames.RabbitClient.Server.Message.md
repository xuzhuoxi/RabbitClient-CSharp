# 命名空间：JLGames.RabbitClient.Server.Message

Rabbit-Server 消息头、读写与请求/响应协议。默认小端。`IRabbitMessageReader` 同时实现 Infra 的 `IDataBufferReader` / `IDataBufferCopier` / `IByteBufferReader` / `IByteBufferCopier`，因此 `RabbitMessageReader` 还提供各基础类型及其数组的读/复制方法。

## IRabbitMessageHeader

消息头通用属性。

- `Extension`：扩展名
- `ProtoId`：协议 Id
- `ClientId`：客户端标识
- `ProtoUid`：协议唯一 Id（`{Extension}:{ProtoId}`）

## IRabbitMessageContent

继承 `IRabbitMessageHeader`，增加原始载荷访问。

- `Content`：消息字节
- `ContentString`：载荷的字符串表示

## IRabbitMessageReader

继承 `IRabbitMessageHeader` 与上述 Buffer 接口。

- `Next`：是否还有未读数据
- `StartReadData()`：重置读位置并读取消息头
- `ReadMessageTo(INetMessage message)` / `CopyMessageTo(INetMessage message)`：读/复制 `INetMessage`（复制不移动读下标）
- `ReadDataTo(ref object data)` / `CopyDataTo(ref object data)`：按 `data` 的运行时类型匹配基础类型、数组或 `INetMessage`
- `CopyRemains()`：复制剩余字节
- `SetMessageBytes(byte[] msg)`：用原始字节更新缓冲区

## IRabbitMessageWriter

继承 `IRabbitMessageHeader`。

- `WriteHeader()`：按当前头字段写入缓存
- `WriteHeader(string extension, string protoId, string uid)`：设置头字段并写入（`uid` 为客户端标识）
- `WriteMessage(INetMessage msg)`：写入 `INetMessage`
- `WriteData(object data)`：写入基础类型及其数组，或 `INetMessage`
- `ToMessageBytes()`：完整编码后的消息字节

## IRabbitRequestMsg

继承 `IRabbitMessageWriter`。

- `SetProtoInfo(string extName, string protoId)`
- `SetClientId(string cid)`
- `StartWriteData()`：写入消息头以开始请求体
- `WriteRequestBase(object baseValue)`：写入基础类型（null 忽略）
- `WriteRequestMessage(INetMessage reqMsg)`：写入消息（null 忽略）

## IRabbitResponseMsg

继承 `IRabbitMessageReader`、`ICloneable<IRabbitResponseMsg>`。

- `RsCode`：响应状态码

## RabbitMessageHeader

公开结构体（非 internal）。

- 字段：`Extension`、`ProtoId`、`ClientId`
- `ProtoUid`：`{Extension}:{ProtoId}`
- `SetHeaderInfo(string extension, string protoId, string cid)`

## RabbitMessageWriter

实现 `IRabbitMessageWriter`。

- 属性：同 `IRabbitMessageHeader`
- 构造函数：`RabbitMessageWriter(bool littleEndian = true)`
- 方法：同接口

## RabbitMessageReader

实现 `IRabbitMessageReader`。

- 属性：同接口（含 `Next`）
- 构造函数：`RabbitMessageReader(bool littleEndian = true)`
- 方法：同接口，另含各基础类型及数组的读/复制方法（来自 Buffer 接口）

## RabbitRequestMsg

继承 `RabbitMessageWriter`，实现 `IRabbitRequestMsg`。

- 构造函数：`RabbitRequestMsg(bool littleEndian = true)`
- 方法：同 `IRabbitRequestMsg`。`StartWriteData()` 内部调用 `WriteHeader()`。

## RabbitResponseMsg

继承 `RabbitMessageReader`，实现 `IRabbitResponseMsg`。

- 属性：`RsCode`
- 构造函数：`RabbitResponseMsg(bool littleEndian = true)`
- `StartReadData()`：先读消息头，再解析 `RsCode`
- `SetMessageBytes(byte[] msg)`
- `Clone()`：克隆当前响应（含未读缓冲区）
