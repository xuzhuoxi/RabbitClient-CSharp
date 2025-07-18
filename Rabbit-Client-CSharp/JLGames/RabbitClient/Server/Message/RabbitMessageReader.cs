using System;
using JLGames.Infra.Buffer;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    public class RabbitMessageReader : IRabbitMessageReader
    {
        internal RabbitHeader m_Header;
        protected readonly INetMessageReader m_Unpacker;

        public string Extension => m_Header.Extension;
        public string ProtoId => m_Header.ProtoId;
        public string ClientId => m_Header.ClientId;
        public string ProtoUid => m_Header.ProtoUid;

        public RabbitMessageReader(bool littleEndian = true)
        {
            m_Header = new RabbitHeader();
            m_Unpacker = new NetMessageReader(littleEndian);
        }

        public bool Next => m_Unpacker.Len > 0;

        public virtual void StartReadData()
        {
            m_Unpacker.SetReadPosition(0);
            var extension = m_Unpacker.ReadString();
            var protoId = m_Unpacker.ReadString();
            var cId = m_Unpacker.ReadString();
            m_Header.SetHeaderInfo(extension, protoId, cId);
        }

        public void ReadMessageTo(INetMessage message)
        {
            var len = m_Unpacker.ReadUInt16();
            var bs = m_Unpacker.ReadBytes(len);
            message.DecodeFromBytes(bs);
        }

        public void CopyMessageTo(INetMessage message)
        {
            var len = m_Unpacker.CopyLen();
            var bs = m_Unpacker.CopyBytes(len, BinarySize.LenSize);
            message.DecodeFromBytes(bs);
        }

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

        public int ReadLen()
        {
            return m_Unpacker.ReadLen();
        }

        public bool ReadBool()
        {
            return m_Unpacker.ReadBool();
        }

        public char ReadChar()
        {
            return m_Unpacker.ReadChar();
        }

        public byte ReadUInt8()
        {
            return m_Unpacker.ReadUInt8();
        }

        public ushort ReadUInt16()
        {
            return m_Unpacker.ReadUInt16();
        }

        public uint ReadUInt32()
        {
            return m_Unpacker.ReadUInt32();
        }

        public ulong ReadUInt64()
        {
            return m_Unpacker.ReadUInt64();
        }

        public sbyte ReadInt8()
        {
            return m_Unpacker.ReadInt8();
        }

        public short ReadInt16()
        {
            return m_Unpacker.ReadInt16();
        }

        public int ReadInt32()
        {
            return m_Unpacker.ReadInt32();
        }

        public long ReadInt64()
        {
            return m_Unpacker.ReadInt64();
        }

        public float ReadFloat()
        {
            return m_Unpacker.ReadFloat();
        }

        public double ReadDouble()
        {
            return m_Unpacker.ReadDouble();
        }

        public string ReadString()
        {
            return m_Unpacker.ReadString();
        }

        public bool[] ReadBoolArray()
        {
            return m_Unpacker.ReadBoolArray();
        }

        public bool[] ReadBoolArray(int num)
        {
            return m_Unpacker.ReadBoolArray(num);
        }

        public char[] ReadCharArray()
        {
            return m_Unpacker.ReadCharArray();
        }

        public char[] ReadCharArray(int num)
        {
            return m_Unpacker.ReadCharArray(num);
        }

        public byte[] ReadUInt8Array()
        {
            return m_Unpacker.ReadUInt8Array();
        }

        public byte[] ReadUInt8Array(int num)
        {
            return m_Unpacker.ReadUInt8Array(num);
        }

        public ushort[] ReadUInt16Array()
        {
            return m_Unpacker.ReadUInt16Array();
        }

        public ushort[] ReadUInt16Array(int num)
        {
            return m_Unpacker.ReadUInt16Array(num);
        }

        public uint[] ReadUInt32Array()
        {
            return m_Unpacker.ReadUInt32Array();
        }

        public uint[] ReadUInt32Array(int num)
        {
            return m_Unpacker.ReadUInt32Array(num);
        }

        public ulong[] ReadUInt64Array()
        {
            return m_Unpacker.ReadUInt64Array();
        }

        public ulong[] ReadUInt64Array(int num)
        {
            return m_Unpacker.ReadUInt64Array(num);
        }

        public sbyte[] ReadInt8Array()
        {
            return m_Unpacker.ReadInt8Array();
        }

        public sbyte[] ReadInt8Array(int num)
        {
            return m_Unpacker.ReadInt8Array(num);
        }

        public short[] ReadInt16Array()
        {
            return m_Unpacker.ReadInt16Array();
        }

        public short[] ReadInt16Array(int num)
        {
            return m_Unpacker.ReadInt16Array(num);
        }

        public int[] ReadInt32Array()
        {
            return m_Unpacker.ReadInt32Array();
        }

        public int[] ReadInt32Array(int num)
        {
            return m_Unpacker.ReadInt32Array(num);
        }

        public long[] ReadInt64Array()
        {
            return m_Unpacker.ReadInt64Array();
        }

        public long[] ReadInt64Array(int num)
        {
            return m_Unpacker.ReadInt64Array(num);
        }

        public float[] ReadFloatArray()
        {
            return m_Unpacker.ReadFloatArray();
        }

        public float[] ReadFloatArray(int num)
        {
            return m_Unpacker.ReadFloatArray(num);
        }

        public double[] ReadDoubleArray()
        {
            return m_Unpacker.ReadDoubleArray();
        }

        public double[] ReadDoubleArray(int num)
        {
            return m_Unpacker.ReadDoubleArray(num);
        }

        public string[] ReadStringArray()
        {
            return m_Unpacker.ReadStringArray();
        }

        public string[] ReadStringArray(int num)
        {
            return m_Unpacker.ReadStringArray(num);
        }

        public void ReadBaseDataTo(ref object data)
        {
            m_Unpacker.ReadBaseDataTo(ref data);
        }

        public string CopyString()
        {
            return m_Unpacker.CopyString();
        }

        public byte[] CopyRemains()
        {
            var copys = m_Unpacker.CopyBytes(m_Unpacker.Len, 0);
            return copys ?? new byte[] { };
        }

        public void SetMessageBytes(byte[] msg)
        {
            m_Unpacker.WriteMessageBytes(msg);
        }

        public int CopyLen(int offset = 0)
        {
            return m_Unpacker.CopyLen(offset);
        }

        public ulong CopyUInt64(int offset = 0)
        {
            return m_Unpacker.CopyUInt64(offset);
        }

        public uint CopyUInt32(int offset = 0)
        {
            return m_Unpacker.CopyUInt32(offset);
        }

        public ushort CopyUInt16(int offset = 0)
        {
            return m_Unpacker.CopyUInt16(offset);
        }

        public byte CopyUInt8(int offset = 0)
        {
            return m_Unpacker.CopyUInt8(offset);
        }

        public long CopyInt64(int offset = 0)
        {
            return m_Unpacker.CopyInt64(offset);
        }

        public int CopyInt32(int offset = 0)
        {
            return m_Unpacker.CopyInt32(offset);
        }

        public short CopyInt16(int offset = 0)
        {
            return m_Unpacker.CopyInt16(offset);
        }

        public sbyte CopyInt8(int offset = 0)
        {
            return m_Unpacker.CopyInt8(offset);
        }

        public bool CopyBool(int offset = 0)
        {
            return m_Unpacker.CopyBool(offset);
        }

        public double CopyDouble(int offset = 0)
        {
            return m_Unpacker.CopyDouble(offset);
        }

        public float CopyFloat(int offset = 0)
        {
            return m_Unpacker.CopyFloat(offset);
        }

        public char CopyChar(int offset = 0)
        {
            return m_Unpacker.CopyChar(offset);
        }

        public string CopyString(int offset = 0)
        {
            return m_Unpacker.CopyString(offset);
        }

        public bool[] CopyBoolArray()
        {
            return m_Unpacker.CopyBoolArray();
        }

        public bool[] CopyBoolArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyBoolArray(num, offset);
        }

        public char[] CopyCharArray()
        {
            return m_Unpacker.CopyCharArray();
        }

        public char[] CopyCharArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyCharArray(num, offset);
        }

        public byte[] CopyUInt8Array()
        {
            return m_Unpacker.CopyUInt8Array();
        }

        public byte[] CopyUInt8Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt8Array(num, offset);
        }

        public ushort[] CopyUInt16Array()
        {
            return m_Unpacker.CopyUInt16Array();
        }

        public ushort[] CopyUInt16Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt16Array(num, offset);
        }

        public uint[] CopyUInt32Array()
        {
            return m_Unpacker.CopyUInt32Array();
        }

        public uint[] CopyUInt32Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt32Array(num, offset);
        }

        public ulong[] CopyUInt64Array()
        {
            return m_Unpacker.CopyUInt64Array();
        }

        public ulong[] CopyUInt64Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyUInt64Array(num, offset);
        }

        public sbyte[] CopyInt8Array()
        {
            return m_Unpacker.CopyInt8Array();
        }

        public sbyte[] CopyInt8Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt8Array(num, offset);
        }

        public short[] CopyInt16Array()
        {
            return m_Unpacker.CopyInt16Array();
        }

        public short[] CopyInt16Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt16Array(num, offset);
        }

        public int[] CopyInt32Array()
        {
            return m_Unpacker.CopyInt32Array();
        }

        public int[] CopyInt32Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt32Array(num, offset);
        }

        public long[] CopyInt64Array()
        {
            return m_Unpacker.CopyInt64Array();
        }

        public long[] CopyInt64Array(int num, int offset = 0)
        {
            return m_Unpacker.CopyInt64Array(num, offset);
        }

        public float[] CopyFloatArray()
        {
            return m_Unpacker.CopyFloatArray();
        }

        public float[] CopyFloatArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyFloatArray(num, offset);
        }

        public double[] CopyDoubleArray()
        {
            return m_Unpacker.CopyDoubleArray();
        }

        public double[] CopyDoubleArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyDoubleArray(num, offset);
        }

        public string[] CopyStringArray()
        {
            return m_Unpacker.CopyStringArray();
        }

        public string[] CopyStringArray(int num, int offset = 0)
        {
            return m_Unpacker.CopyStringArray(num, offset);
        }

        public void CopyBaseDataTo(ref object data)
        {
            m_Unpacker.CopyBaseDataTo(ref data);
        }

        public int Len => m_Unpacker.Len;

        public int ReadPosition => m_Unpacker.ReadPosition;

        public void SetReadPosition(int pos)
        {
            m_Unpacker.SetReadPosition(pos);
        }

        public byte ReadByte()
        {
            return m_Unpacker.ReadByte();
        }

        public byte[] ReadBytes()
        {
            return m_Unpacker.ReadBytes();
        }

        public byte[] ReadBytes(int size)
        {
            return m_Unpacker.ReadBytes(size);
        }

        public int ReadBytesTo(ref byte[] dst)
        {
            return m_Unpacker.ReadBytesTo(ref dst);
        }

        public int ReadBytesTo(ref byte[] dst, int size)
        {
            return m_Unpacker.ReadBytesTo(ref dst, size);
        }

        public byte CopyByte()
        {
            return m_Unpacker.CopyByte();
        }

        public byte[] CopyBytes(int offset = 0)
        {
            return m_Unpacker.CopyBytes(offset);
        }

        public byte[] CopyBytes(int size, int offset)
        {
            return m_Unpacker.CopyBytes(size, offset);
        }

        public int CopyBytesTo(ref byte[] dst, int offset = 0)
        {
            return m_Unpacker.CopyBytesTo(ref dst, offset);
        }

        public int CopyBytesTo(ref byte[] dst, int size, int offset)
        {
            return m_Unpacker.CopyBytesTo(ref dst, size, offset);
        }
    }
}
