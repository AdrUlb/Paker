using System.Buffers.Binary;
using System.Text;

namespace PopLib.Misc;

internal static class StreamExtensions
{
	internal static SubStream SubStream(this Stream stream, int offset, int length, bool forceReadOnly) => new(stream, offset, length, forceReadOnly);

	internal static int ReadInt(this Stream stream)
	{
		Span<byte> buf = stackalloc byte[4];
		stream.ReadExactly(buf);
		return BinaryPrimitives.ReadInt32LittleEndian(buf);
	}

	internal static uint ReadUint(this Stream stream)
	{
		Span<byte> buf = stackalloc byte[4];
		stream.ReadExactly(buf);
		return BinaryPrimitives.ReadUInt32LittleEndian(buf);
	}

	internal static float ReadFloat(this Stream stream)
	{
		Span<byte> buf = stackalloc byte[4];
		stream.ReadExactly(buf);
		return BinaryPrimitives.ReadSingleLittleEndian(buf);
	}

	internal static string ReadString(this Stream stream)
	{
		var length = stream.ReadInt();
		Span<byte> buf = stackalloc byte[length];
		stream.ReadExactly(buf);
		return Encoding.UTF8.GetString(buf);
	}

	internal static void WriteUint(this Stream stream, uint value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteUInt32LittleEndian(buf, value);
		stream.Write(buf);
	}

	internal static void WriteInt(this Stream stream, int value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteInt32LittleEndian(buf, value);
		stream.Write(buf);
	}

	internal static void WriteFloat(this Stream stream, float value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteSingleLittleEndian(buf, value);
		stream.Write(buf);
	}

	internal static void WriteString(this Stream stream, string str)
	{
		stream.WriteInt(str.Length);
		Span<byte> buf = stackalloc byte[str.Length];
		Encoding.UTF8.GetBytes(str, buf);
		stream.Write(buf);
	}
}
