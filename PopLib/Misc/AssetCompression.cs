using System.IO.Compression;

namespace PopLib.Misc;

public static class AssetCompression
{
	public static void Compress(Stream inputStream, Stream outputStream)
	{
		outputStream.WriteUint(0xDEADFED4);
		outputStream.WriteUint((uint)inputStream.Length);

		using var zlibStream = new ZLibStream(outputStream, CompressionMode.Compress);
		inputStream.CopyTo(zlibStream);
	}

	public static void Decompress(Stream inputStream, Stream outputStream)
	{
		if (inputStream.ReadUint() != 0xDEADFED4)
			throw new("FIXME");

		var decompressedSize = inputStream.ReadInt();
		
		using var zlibStream = new ZLibStream(inputStream, CompressionMode.Decompress);
		zlibStream.CopyTo(outputStream);
	}
}
