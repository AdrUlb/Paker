namespace PakLib;

internal static class StreamExtensions
{
	internal static Substream Substream(this Stream stream, int offset, int length, bool forceReadOnly) => new(stream, offset, length, forceReadOnly);
}
