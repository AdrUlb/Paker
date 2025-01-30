namespace PopLib.Misc;

internal static class StreamExtensions
{
	internal static SubStream SubStream(this Stream stream, int offset, int length, bool forceReadOnly) => new(stream, offset, length, forceReadOnly);
}
