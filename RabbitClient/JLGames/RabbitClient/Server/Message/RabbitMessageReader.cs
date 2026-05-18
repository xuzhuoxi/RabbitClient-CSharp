using System;
using JLGames.Infra.Buffer;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Default implementation of Rabbit message reading.
    /// Rabbit消息读取默认实现。
    /// </summary>
    public class RabbitMessageReader : IRabbitMessageReader
    {
        internal RabbitMessageHeader m_MessageHeader;
        protected readonly INetMessageReader m_Unpacker;

        /// <summary>
        /// Extension name from the message header.
        /// 消息头中的扩展名。
        /// </summary>
        public string Extension => m_MessageHeader.Extension;

        /// <summary>
        /// Protocol identifier from the message header.
        /// 消息头中的协议Id。
        /// </summary>
        public string ProtoId => m_MessageHeader.ProtoId;

        /// <summary>
        /// Client identifier from the message header.
        /// 消息头中的客户端标识。
        /// </summary>
        public string ClientId => m_MessageHeader.ClientId;

        /// <summary>
        /// Unique protocol id from the message header.
        /// 消息头中的协议唯一Id。
        /// </summary>
        public string ProtoUid => m_MessageHeader.ProtoUid;

        /// <summary>
        /// Creates a message reader with optional little-endian byte order.
        /// 创建消息读取器，可选小端字节序。
        /// </summary>
        /// <param name="littleEndian">Whether to use little-endian decoding<br/>是否使用小端解码</param>
        public RabbitMessageReader(bool littleEndian = true)
        {
            m_MessageHeader = new RabbitMessageHeader();
            m_Unpacker = new NetMessageReader(littleEndian);
        }

        /// <summary>
        /// Whether more data remains in the buffer beyond the current read position.
        /// 缓冲区在当前读取位置之后是否仍有可读数据。
        /// </summary>
        public bool Next => m_Unpacker.Len > 0;

        /// <summary>
        /// Resets the read position to the start and reads header fields into the message header.
        /// 将读取位置重置到起始处并读取消息头字段到消息头对象。
        /// </summary>
        public virtual void StartReadData()
        {
            m_Unpacker.SetReadPosition(0);
            var extension = m_Unpacker.ReadString();
            var protoId = m_Unpacker.ReadString();
            var cId = m_Unpacker.ReadString();
            m_MessageHeader.SetHeaderInfo(extension, protoId, cId);
        }

        /// <summary>
        /// Reads length-prefixed message bytes and decodes them into the target message.
        /// 读取长度前缀的消息字节并解码到目标消息。
        /// </summary>
        /// <param name="message">Target net message<br/>目标网络消息</param>
        public void ReadMessageTo(INetMessage message)
        {
            var len = m_Unpacker.ReadUInt16();
            var bs = m_Unpacker.ReadBytes(len);
            message.DecodeFromBytes(bs);
        }

        /// <summary>
        /// Copies length-prefixed message bytes without advancing consumption and decodes into the target message.
        /// 复制长度前缀的消息字节且不推进消费位置，并解码到目标消息。
        /// </summary>
        /// <param name="message">Target net message<br/>目标网络消息</param>
        public void CopyMessageTo(INetMessage message)
        {
            var len = m_Unpacker.CopyLen();
            var bs = m_Unpacker.CopyBytes(len, BinarySize.LenSize);
            message.DecodeFromBytes(bs);
        }

        /// <summary>
        /// Reads supported base data or nested net messages into the provided reference holder.
        /// 将支持的基元数据或嵌套网络消息读入所提供的引用持有者。
        /// </summary>
        /// <param name="data">Value holder; updated with decoded content<br/>值持有者；将更新为解码后的内容</param>
        public void ReadDataTo(ref object data)
        {
            if (data is INetMessage message)
            {
                m_Unpacker.ReadMessageTo(ref message);
                return;
            }

            try
            {
                m_Unpacker.ReadBaseDataTo(ref data);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Copies supported base data or nested net messages into the holder without advancing consumption.
        /// 在不推进消费的情况下，将支持的基元数据或嵌套网络消息复制到持有者。
        /// </summary>
        /// <param name="data">Value holder; updated with decoded content<br/>值持有者；将更新为解码后的内容</param>
        public void CopyDataTo(ref object data)
        {
            if (data is INetMessage message)
            {
                m_Unpacker.CopyMessageTo(ref message);
                return;
            }

            try
            {
                m_Unpacker.CopyBaseDataTo(ref data);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        // Delegate -------------

        /// <summary>
        /// Reads the packed byte length prefix at the current position.
        /// 读取当前位置处的打包字节长度前缀。
        /// </summary>
        /// <returns>Decoded length value<br/>解码后的长度值</returns>
        public int ReadLen()
        {
            return m_Unpacker.ReadLen();
        }

        /// <summary>
        /// Reads a boolean value from the buffer.
        /// 从缓冲区读取布尔值。
        /// </summary>
        /// <returns>Decoded boolean<br/>解码后的布尔值</returns>
        public bool ReadBool()
        {
            return m_Unpacker.ReadBool();
        }

        /// <summary>
        /// Reads a character value from the buffer.
        /// 从缓冲区读取字符值。
        /// </summary>
        /// <returns>Decoded character<br/>解码后的字符</returns>
        public char ReadChar()
        {
            return m_Unpacker.ReadChar();
        }

        /// <summary>
        /// Reads an unsigned 8-bit integer from the buffer.
        /// 从缓冲区读取无符号8位整数。
        /// </summary>
        /// <returns>Decoded byte<br/>解码后的字节</returns>
        public byte ReadUInt8()
        {
            return m_Unpacker.ReadUInt8();
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer from the buffer.
        /// 从缓冲区读取无符号16位整数。
        /// </summary>
        /// <returns>Decoded ushort value<br/>解码后的ushort值</returns>
        public ushort ReadUInt16()
        {
            return m_Unpacker.ReadUInt16();
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from the buffer.
        /// 从缓冲区读取无符号32位整数。
        /// </summary>
        /// <returns>Decoded uint value<br/>解码后的uint值</returns>
        public uint ReadUInt32()
        {
            return m_Unpacker.ReadUInt32();
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer from the buffer.
        /// 从缓冲区读取无符号64位整数。
        /// </summary>
        /// <returns>Decoded ulong value<br/>解码后的ulong值</returns>
        public ulong ReadUInt64()
        {
            return m_Unpacker.ReadUInt64();
        }

        /// <summary>
        /// Reads a signed 8-bit integer from the buffer.
        /// 从缓冲区读取有符号8位整数。
        /// </summary>
        /// <returns>Decoded sbyte value<br/>解码后的sbyte值</returns>
        public sbyte ReadInt8()
        {
            return m_Unpacker.ReadInt8();
        }

        /// <summary>
        /// Reads a signed 16-bit integer from the buffer.
        /// 从缓冲区读取有符号16位整数。
        /// </summary>
        /// <returns>Decoded short value<br/>解码后的short值</returns>
        public short ReadInt16()
        {
            return m_Unpacker.ReadInt16();
        }

        /// <summary>
        /// Reads a signed 32-bit integer from the buffer.
        /// 从缓冲区读取有符号32位整数。
        /// </summary>
        /// <returns>Decoded int value<br/>解码后的int值</returns>
        public int ReadInt32()
        {
            return m_Unpacker.ReadInt32();
        }

        /// <summary>
        /// Reads a signed 64-bit integer from the buffer.
        /// 从缓冲区读取有符号64位整数。
        /// </summary>
        /// <returns>Decoded long value<br/>解码后的long值</returns>
        public long ReadInt64()
        {
            return m_Unpacker.ReadInt64();
        }

        /// <summary>
        /// Reads a single-precision floating-point value from the buffer.
        /// 从缓冲区读取单精度浮点值。
        /// </summary>
        /// <returns>Decoded float value<br/>解码后的float值</returns>
        public float ReadFloat()
        {
            return m_Unpacker.ReadFloat();
        }

        /// <summary>
        /// Reads a double-precision floating-point value from the buffer.
        /// 从缓冲区读取双精度浮点值。
        /// </summary>
        /// <returns>Decoded double value<br/>解码后的double值</returns>
        public double ReadDouble()
        {
            return m_Unpacker.ReadDouble();
        }

        /// <summary>
        /// Reads a length-prefixed string from the buffer.
        /// 从缓冲区读取带长度前缀的字符串。
        /// </summary>
        /// <returns>Decoded string<br/>解码后的字符串</returns>
        public string ReadString()
        {
            return m_Unpacker.ReadString();
        }

        /// <summary>
        /// Reads a length-prefixed boolean array from the buffer.
        /// 从缓冲区读取带长度前缀的布尔数组。
        /// </summary>
        /// <returns>Decoded boolean array<br/>解码后的布尔数组</returns>
        public bool[] ReadBoolArray()
        {
            return m_Unpacker.ReadBoolArray();
        }

        /// <summary>
        /// Reads a boolean array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的布尔数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded boolean array<br/>解码后的布尔数组</returns>
        public bool[] ReadBoolArray(int num)
        {
            return m_Unpacker.ReadBoolArray(num);
        }

        /// <summary>
        /// Reads a length-prefixed character array from the buffer.
        /// 从缓冲区读取带长度前缀的字符数组。
        /// </summary>
        /// <returns>Decoded character array<br/>解码后的字符数组</returns>
        public char[] ReadCharArray()
        {
            return m_Unpacker.ReadCharArray();
        }

        /// <summary>
        /// Reads a character array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的字符数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded character array<br/>解码后的字符数组</returns>
        public char[] ReadCharArray(int num)
        {
            return m_Unpacker.ReadCharArray(num);
        }

        /// <summary>
        /// Reads a length-prefixed byte array from the buffer.
        /// 从缓冲区读取带长度前缀的字节数组。
        /// </summary>
        /// <returns>Decoded byte array<br/>解码后的字节数组</returns>
        public byte[] ReadUInt8Array()
        {
            return m_Unpacker.ReadUInt8Array();
        }

        /// <summary>
        /// Reads a byte array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的字节数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded byte array<br/>解码后的字节数组</returns>
        public byte[] ReadUInt8Array(int num)
        {
            return m_Unpacker.ReadUInt8Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed ushort array from the buffer.
        /// 从缓冲区读取带长度前缀的ushort数组。
        /// </summary>
        /// <returns>Decoded ushort array<br/>解码后的ushort数组</returns>
        public ushort[] ReadUInt16Array()
        {
            return m_Unpacker.ReadUInt16Array();
        }

        /// <summary>
        /// Reads a ushort array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的ushort数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded ushort array<br/>解码后的ushort数组</returns>
        public ushort[] ReadUInt16Array(int num)
        {
            return m_Unpacker.ReadUInt16Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed uint array from the buffer.
        /// 从缓冲区读取带长度前缀的uint数组。
        /// </summary>
        /// <returns>Decoded uint array<br/>解码后的uint数组</returns>
        public uint[] ReadUInt32Array()
        {
            return m_Unpacker.ReadUInt32Array();
        }

        /// <summary>
        /// Reads a uint array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的uint数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded uint array<br/>解码后的uint数组</returns>
        public uint[] ReadUInt32Array(int num)
        {
            return m_Unpacker.ReadUInt32Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed ulong array from the buffer.
        /// 从缓冲区读取带长度前缀的ulong数组。
        /// </summary>
        /// <returns>Decoded ulong array<br/>解码后的ulong数组</returns>
        public ulong[] ReadUInt64Array()
        {
            return m_Unpacker.ReadUInt64Array();
        }

        /// <summary>
        /// Reads a ulong array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的ulong数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded ulong array<br/>解码后的ulong数组</returns>
        public ulong[] ReadUInt64Array(int num)
        {
            return m_Unpacker.ReadUInt64Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed sbyte array from the buffer.
        /// 从缓冲区读取带长度前缀的sbyte数组。
        /// </summary>
        /// <returns>Decoded sbyte array<br/>解码后的sbyte数组</returns>
        public sbyte[] ReadInt8Array()
        {
            return m_Unpacker.ReadInt8Array();
        }

        /// <summary>
        /// Reads an sbyte array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的sbyte数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded sbyte array<br/>解码后的sbyte数组</returns>
        public sbyte[] ReadInt8Array(int num)
        {
            return m_Unpacker.ReadInt8Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed short array from the buffer.
        /// 从缓冲区读取带长度前缀的short数组。
        /// </summary>
        /// <returns>Decoded short array<br/>解码后的short数组</returns>
        public short[] ReadInt16Array()
        {
            return m_Unpacker.ReadInt16Array();
        }

        /// <summary>
        /// Reads a short array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的short数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded short array<br/>解码后的short数组</returns>
        public short[] ReadInt16Array(int num)
        {
            return m_Unpacker.ReadInt16Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed int array from the buffer.
        /// 从缓冲区读取带长度前缀的int数组。
        /// </summary>
        /// <returns>Decoded int array<br/>解码后的int数组</returns>
        public int[] ReadInt32Array()
        {
            return m_Unpacker.ReadInt32Array();
        }

        /// <summary>
        /// Reads an int array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的int数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded int array<br/>解码后的int数组</returns>
        public int[] ReadInt32Array(int num)
        {
            return m_Unpacker.ReadInt32Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed long array from the buffer.
        /// 从缓冲区读取带长度前缀的long数组。
        /// </summary>
        /// <returns>Decoded long array<br/>解码后的long数组</returns>
        public long[] ReadInt64Array()
        {
            return m_Unpacker.ReadInt64Array();
        }

        /// <summary>
        /// Reads a long array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的long数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded long array<br/>解码后的long数组</returns>
        public long[] ReadInt64Array(int num)
        {
            return m_Unpacker.ReadInt64Array(num);
        }

        /// <summary>
        /// Reads a length-prefixed float array from the buffer.
        /// 从缓冲区读取带长度前缀的float数组。
        /// </summary>
        /// <returns>Decoded float array<br/>解码后的float数组</returns>
        public float[] ReadFloatArray()
        {
            return m_Unpacker.ReadFloatArray();
        }

        /// <summary>
        /// Reads a float array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的float数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded float array<br/>解码后的float数组</returns>
        public float[] ReadFloatArray(int num)
        {
            return m_Unpacker.ReadFloatArray(num);
        }

        /// <summary>
        /// Reads a length-prefixed double array from the buffer.
        /// 从缓冲区读取带长度前缀的double数组。
        /// </summary>
        /// <returns>Decoded double array<br/>解码后的double数组</returns>
        public double[] ReadDoubleArray()
        {
            return m_Unpacker.ReadDoubleArray();
        }

        /// <summary>
        /// Reads a double array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的double数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded double array<br/>解码后的double数组</returns>
        public double[] ReadDoubleArray(int num)
        {
            return m_Unpacker.ReadDoubleArray(num);
        }

        /// <summary>
        /// Reads a length-prefixed string array from the buffer.
        /// 从缓冲区读取带长度前缀的字符串数组。
        /// </summary>
        /// <returns>Decoded string array<br/>解码后的字符串数组</returns>
        public string[] ReadStringArray()
        {
            return m_Unpacker.ReadStringArray();
        }

        /// <summary>
        /// Reads a string array with the specified element count from the buffer.
        /// 从缓冲区读取指定元素个数的字符串数组。
        /// </summary>
        /// <param name="num">Number of elements to read<br/>要读取的元素个数</param>
        /// <returns>Decoded string array<br/>解码后的字符串数组</returns>
        public string[] ReadStringArray(int num)
        {
            return m_Unpacker.ReadStringArray(num);
        }

        /// <summary>
        /// Reads polymorphic base data into the holder using the unpacker rules.
        /// 按解包规则将多态基元数据读入持有者。
        /// </summary>
        /// <param name="data">Value holder; updated with decoded content<br/>值持有者；将更新为解码后的内容</param>
        public void ReadBaseDataTo(ref object data)
        {
            m_Unpacker.ReadBaseDataTo(ref data);
        }

        /// <summary>
        /// Copies a length-prefixed string without advancing the read position.
        /// 复制带长度前缀的字符串且不推进读取位置。
        /// </summary>
        /// <returns>Copied string value<br/>复制得到的字符串值</returns>
        public string CopyString()
        {
            return m_Unpacker.CopyString();
        }

        /// <summary>
        /// Copies all remaining bytes without advancing the read position.
        /// 复制剩余全部字节且不推进读取位置。
        /// </summary>
        /// <returns>Remaining bytes, or an empty array when none<br/>剩余字节；若无则返回空数组</returns>
        public byte[] CopyRemains()
        {
            var copys = m_Unpacker.CopyBytes(m_Unpacker.Len, 0);
            return copys ?? new byte[] { };
        }

        /// <summary>
        /// Loads raw message bytes into the underlying reader buffer for subsequent reads.
        /// 将原始消息字节加载到底层读取缓冲区以供后续读取。
        /// </summary>
        /// <param name="msg">Complete message bytes<br/>完整消息字节</param>
        public void SetMessageBytes(byte[] msg)
        {
            m_Unpacker.WriteMessageBytes(msg);
        }

        /// <summary>
        /// Copies the packed length prefix at the given offset without advancing consumption.
        /// 在给定偏移处复制打包长度前缀且不推进消费位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Decoded length value<br/>解码后的长度值</returns>
        public int CopyLen(int offset = 0)
        {
            return m_Unpacker.CopyLen(offset);
        }

        /// <summary>
        /// Copies an unsigned 64-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制无符号64位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied ulong value<br/>复制得到的ulong值</returns>
        public ulong CopyUInt64(int offset = 0)
        {
            return m_Unpacker.CopyUInt64(offset);
        }

        /// <summary>
        /// Copies an unsigned 32-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制无符号32位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied uint value<br/>复制得到的uint值</returns>
        public uint CopyUInt32(int offset = 0)
        {
            return m_Unpacker.CopyUInt32(offset);
        }

        /// <summary>
        /// Copies an unsigned 16-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制无符号16位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied ushort value<br/>复制得到的ushort值</returns>
        public ushort CopyUInt16(int offset = 0)
        {
            return m_Unpacker.CopyUInt16(offset);
        }

        /// <summary>
        /// Copies an unsigned 8-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制无符号8位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied byte value<br/>复制得到的字节值</returns>
        public byte CopyUInt8(int offset = 0)
        {
            return m_Unpacker.CopyUInt8(offset);
        }

        /// <summary>
        /// Copies a signed 64-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制有符号64位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied long value<br/>复制得到的long值</returns>
        public long CopyInt64(int offset = 0)
        {
            return m_Unpacker.CopyInt64(offset);
        }

        /// <summary>
        /// Copies a signed 32-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制有符号32位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied int value<br/>复制得到的int值</returns>
        public int CopyInt32(int offset = 0)
        {
            return m_Unpacker.CopyInt32(offset);
        }

        /// <summary>
        /// Copies a signed 16-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制有符号16位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied short value<br/>复制得到的short值</returns>
        public short CopyInt16(int offset = 0)
        {
            return m_Unpacker.CopyInt16(offset);
        }

        /// <summary>
        /// Copies a signed 8-bit integer at the given offset without advancing the read position.
        /// 在给定偏移处复制有符号8位整数且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied sbyte value<br/>复制得到的sbyte值</returns>
        public sbyte CopyInt8(int offset = 0)
        {
            return m_Unpacker.CopyInt8(offset);
        }

        /// <summary>
        /// Copies a boolean value at the given offset without advancing the read position.
        /// 在给定偏移处复制布尔值且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied boolean value<br/>复制得到的布尔值</returns>
        public bool CopyBool(int offset = 0)
        {
            return m_Unpacker.CopyBool(offset);
        }

        /// <summary>
        /// Copies a double-precision floating-point value at the given offset without advancing the read position.
        /// 在给定偏移处复制双精度浮点值且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied double value<br/>复制得到的double值</returns>
        public double CopyDouble(int offset = 0)
        {
            return m_Unpacker.CopyDouble(offset);
        }

        /// <summary>
        /// Copies a single-precision floating-point value at the given offset without advancing the read position.
        /// 在给定偏移处复制单精度浮点值且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied float value<br/>复制得到的float值</returns>
        public float CopyFloat(int offset = 0)
        {
            return m_Unpacker.CopyFloat(offset);
        }

        /// <summary>
        /// Copies a character value at the given offset without advancing the read position.
        /// 在给定偏移处复制字符值且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied character value<br/>复制得到的字符值</returns>
        public char CopyChar(int offset = 0)
        {
            return m_Unpacker.CopyChar(offset);
        }

        /// <summary>
        /// Copies a length-prefixed string at the given offset without advancing the read position.
        /// 在给定偏移处复制带长度前缀的字符串且不推进读取位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied string value<br/>复制得到的字符串值</returns>
        public string CopyString(int offset = 0)
        {
            return m_Unpacker.CopyString(offset);
        }

        /// <summary>
        /// Copies a length-prefixed boolean array without advancing the read position.
        /// 复制带长度前缀的布尔数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied boolean array<br/>复制得到的布尔数组</returns>
        public bool[] CopyBoolArray()
        {
            return m_Unpacker.CopyBoolArray();
        }

        /// <summary>
        /// Copies a boolean array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制布尔数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied boolean array<br/>复制得到的布尔数组</returns>
        public bool[] CopyBoolArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyBoolArray(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed character array without advancing the read position.
        /// 复制带长度前缀的字符数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied character array<br/>复制得到的字符数组</returns>
        public char[] CopyCharArray()
        {
            return m_Unpacker.CopyCharArray();
        }

        /// <summary>
        /// Copies a character array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制字符数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied character array<br/>复制得到的字符数组</returns>
        public char[] CopyCharArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyCharArray(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed byte array without advancing the read position.
        /// 复制带长度前缀的字节数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied byte array<br/>复制得到的字节数组</returns>
        public byte[] CopyUInt8Array()
        {
            return m_Unpacker.CopyUInt8Array();
        }

        /// <summary>
        /// Copies a byte array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制字节数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied byte array<br/>复制得到的字节数组</returns>
        public byte[] CopyUInt8Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt8Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed ushort array without advancing the read position.
        /// 复制带长度前缀的ushort数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied ushort array<br/>复制得到的ushort数组</returns>
        public ushort[] CopyUInt16Array()
        {
            return m_Unpacker.CopyUInt16Array();
        }

        /// <summary>
        /// Copies a ushort array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制ushort数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied ushort array<br/>复制得到的ushort数组</returns>
        public ushort[] CopyUInt16Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt16Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed uint array without advancing the read position.
        /// 复制带长度前缀的uint数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied uint array<br/>复制得到的uint数组</returns>
        public uint[] CopyUInt32Array()
        {
            return m_Unpacker.CopyUInt32Array();
        }

        /// <summary>
        /// Copies a uint array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制uint数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied uint array<br/>复制得到的uint数组</returns>
        public uint[] CopyUInt32Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt32Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed ulong array without advancing the read position.
        /// 复制带长度前缀的ulong数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied ulong array<br/>复制得到的ulong数组</returns>
        public ulong[] CopyUInt64Array()
        {
            return m_Unpacker.CopyUInt64Array();
        }

        /// <summary>
        /// Copies a ulong array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制ulong数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied ulong array<br/>复制得到的ulong数组</returns>
        public ulong[] CopyUInt64Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt64Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed sbyte array without advancing the read position.
        /// 复制带长度前缀的sbyte数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied sbyte array<br/>复制得到的sbyte数组</returns>
        public sbyte[] CopyInt8Array()
        {
            return m_Unpacker.CopyInt8Array();
        }

        /// <summary>
        /// Copies an sbyte array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制sbyte数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied sbyte array<br/>复制得到的sbyte数组</returns>
        public sbyte[] CopyInt8Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt8Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed short array without advancing the read position.
        /// 复制带长度前缀的short数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied short array<br/>复制得到的short数组</returns>
        public short[] CopyInt16Array()
        {
            return m_Unpacker.CopyInt16Array();
        }

        /// <summary>
        /// Copies a short array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制short数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied short array<br/>复制得到的short数组</returns>
        public short[] CopyInt16Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt16Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed int array without advancing the read position.
        /// 复制带长度前缀的int数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied int array<br/>复制得到的int数组</returns>
        public int[] CopyInt32Array()
        {
            return m_Unpacker.CopyInt32Array();
        }

        /// <summary>
        /// Copies an int array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制int数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied int array<br/>复制得到的int数组</returns>
        public int[] CopyInt32Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt32Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed long array without advancing the read position.
        /// 复制带长度前缀的long数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied long array<br/>复制得到的long数组</returns>
        public long[] CopyInt64Array()
        {
            return m_Unpacker.CopyInt64Array();
        }

        /// <summary>
        /// Copies a long array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制long数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied long array<br/>复制得到的long数组</returns>
        public long[] CopyInt64Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt64Array(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed float array without advancing the read position.
        /// 复制带长度前缀的float数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied float array<br/>复制得到的float数组</returns>
        public float[] CopyFloatArray()
        {
            return m_Unpacker.CopyFloatArray();
        }

        /// <summary>
        /// Copies a float array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制float数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied float array<br/>复制得到的float数组</returns>
        public float[] CopyFloatArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyFloatArray(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed double array without advancing the read position.
        /// 复制带长度前缀的double数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied double array<br/>复制得到的double数组</returns>
        public double[] CopyDoubleArray()
        {
            return m_Unpacker.CopyDoubleArray();
        }

        /// <summary>
        /// Copies a double array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制double数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied double array<br/>复制得到的double数组</returns>
        public double[] CopyDoubleArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyDoubleArray(num, offset);
        }

        /// <summary>
        /// Copies a length-prefixed string array without advancing the read position.
        /// 复制带长度前缀的字符串数组且不推进读取位置。
        /// </summary>
        /// <returns>Copied string array<br/>复制得到的字符串数组</returns>
        public string[] CopyStringArray()
        {
            return m_Unpacker.CopyStringArray();
        }

        /// <summary>
        /// Copies a string array with the specified count and offset without advancing the read position.
        /// 按指定数量与偏移复制字符串数组且不推进读取位置。
        /// </summary>
        /// <param name="num">Number of elements to copy<br/>要复制的元素个数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied string array<br/>复制得到的字符串数组</returns>
        public string[] CopyStringArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyStringArray(num, offset);
        }

        /// <summary>
        /// Copies polymorphic base data into the holder without advancing consumption.
        /// 在不推进消费的情况下，将多态基元数据复制到持有者。
        /// </summary>
        /// <param name="data">Value holder; updated with decoded content<br/>值持有者；将更新为解码后的内容</param>
        public void CopyBaseDataTo(ref object data)
        {
            m_Unpacker.CopyBaseDataTo(ref data);
        }

        /// <summary>
        /// Number of bytes remaining after the current read position.
        /// 当前读取位置之后剩余的字节数。
        /// </summary>
        public int Len => m_Unpacker.Len;

        /// <summary>
        /// Current read cursor position within the buffer.
        /// 缓冲区内的当前读取游标位置。
        /// </summary>
        public int ReadPosition => m_Unpacker.ReadPosition;

        /// <summary>
        /// Sets the read cursor to the specified buffer offset.
        /// 将读取游标设置到指定的缓冲区偏移。
        /// </summary>
        /// <param name="pos">Zero-based byte offset for the next read<br/>下次读取的从零开始的字节偏移</param>
        public void SetReadPosition(int pos)
        {
            m_Unpacker.SetReadPosition(pos);
        }

        /// <summary>
        /// Reads a single raw byte from the buffer.
        /// 从缓冲区读取单个原始字节。
        /// </summary>
        /// <returns>Byte value<br/>字节值</returns>
        public byte ReadByte()
        {
            return m_Unpacker.ReadByte();
        }

        /// <summary>
        /// Reads a byte array using the unpacker's byte-array rules at the current position.
        /// 按解包器的字节数组规则从当前位置读取字节数组。
        /// </summary>
        /// <returns>Read byte array<br/>读取到的字节数组</returns>
        public byte[] ReadBytes()
        {
            return m_Unpacker.ReadBytes();
        }

        /// <summary>
        /// Reads the specified number of bytes from the buffer.
        /// 从缓冲区读取指定数量的字节。
        /// </summary>
        /// <param name="size">Number of bytes to read<br/>要读取的字节数</param>
        /// <returns>Read byte array<br/>读取到的字节数组</returns>
        public byte[] ReadBytes(int size)
        {
            return m_Unpacker.ReadBytes(size);
        }

        /// <summary>
        /// Reads bytes into the destination buffer and returns how many bytes were consumed.
        /// 将字节读入目标缓冲区并返回消耗的字节数。
        /// </summary>
        /// <param name="dst">Destination buffer reference<br/>目标缓冲区引用</param>
        /// <returns>Number of bytes read<br/>读取的字节数</returns>
        public int ReadBytesTo(ref byte[] dst)
        {
            return m_Unpacker.ReadBytesTo(ref dst);
        }

        /// <summary>
        /// Reads up to the specified size into the destination buffer and returns how many bytes were consumed.
        /// 最多读取指定数量到目标缓冲区并返回消耗的字节数。
        /// </summary>
        /// <param name="dst">Destination buffer reference<br/>目标缓冲区引用</param>
        /// <param name="size">Maximum number of bytes to read<br/>最大读取字节数</param>
        /// <returns>Number of bytes read<br/>读取的字节数</returns>
        public int ReadBytesTo(ref byte[] dst, int size)
        {
            return m_Unpacker.ReadBytesTo(ref dst, size);
        }

        /// <summary>
        /// Copies a raw byte at the current read position without advancing consumption.
        /// 在当前读取位置复制原始字节且不推进消费位置。
        /// </summary>
        /// <returns>Copied byte value<br/>复制得到的字节值</returns>
        public byte CopyByte()
        {
            return m_Unpacker.CopyByte();
        }

        /// <summary>
        /// Copies bytes starting at an offset relative to the read cursor without advancing consumption.
        /// 从相对于读取游标的偏移开始复制字节且不推进消费位置。
        /// </summary>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied bytes<br/>复制得到的字节</returns>
        public byte[] CopyBytes(int offset = 0)
        {
            return m_Unpacker.CopyBytes(offset);
        }

        /// <summary>
        /// Copies a fixed number of bytes at the specified offset without advancing consumption.
        /// 在指定偏移处复制固定数量的字节且不推进消费位置。
        /// </summary>
        /// <param name="size">Number of bytes to copy<br/>要复制的字节数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Copied bytes<br/>复制得到的字节</returns>
        public byte[] CopyBytes(int size, int offset)
        {
            return m_Unpacker.CopyBytes(size, offset);
        }

        /// <summary>
        /// Copies bytes into the destination buffer at an optional offset without advancing consumption.
        /// 在可选偏移处将字节复制到目标缓冲区且不推进消费位置。
        /// </summary>
        /// <param name="dst">Destination buffer reference<br/>目标缓冲区引用</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Number of bytes copied<br/>复制的字节数</returns>
        public int CopyBytesTo(ref byte[] dst, int offset = 0)
        {
            return m_Unpacker.CopyBytesTo(ref dst, offset);
        }

        /// <summary>
        /// Copies a fixed number of bytes into the destination buffer at the given offset without advancing consumption.
        /// 在给定偏移处将固定数量的字节复制到目标缓冲区且不推进消费位置。
        /// </summary>
        /// <param name="dst">Destination buffer reference<br/>目标缓冲区引用</param>
        /// <param name="size">Number of bytes to copy<br/>要复制的字节数</param>
        /// <param name="offset">Byte offset relative to the current read position<br/>相对当前读取位置的字节偏移</param>
        /// <returns>Number of bytes copied<br/>复制的字节数</returns>
        public int CopyBytesTo(ref byte[] dst, int size, int offset)
        {
            return m_Unpacker.CopyBytesTo(ref dst, size, offset);
        }
    }
}
