using PopLib.Reanim;

var files = Directory.GetFiles("reanim", "*", SearchOption.AllDirectories);
if (!Directory.Exists(Path.Combine("compiled", "reanim")))
	Directory.CreateDirectory(Path.Combine("compiled", "reanim"));

foreach (var file in files)
{
	if (!file.EndsWith(".reanim"))
		throw new("FIXME");

	var destFile = Path.Combine("compiled", file + ".compiled");

	using var fs = File.OpenRead(file);
	var reanim = ReanimXmlReader.ReadFromStream(fs);

	using var fs2 = File.OpenWrite(destFile);
	ReanimBinaryWriter.WriteToStream(reanim, fs2);
}
