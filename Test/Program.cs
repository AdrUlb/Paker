using PopLib.Particles;
using PopLib.Reanim;

{
	var files = Directory.GetFiles("reanim", "*.reanim", SearchOption.AllDirectories);
	if (!Directory.Exists(Path.Combine("compiled", "reanim")))
		Directory.CreateDirectory(Path.Combine("compiled", "reanim"));

	foreach (var file in files)
	{
		if (!file.EndsWith(".reanim"))
			throw new("FIXME");

		var destFile = Path.Combine("compiled", file + ".compiled");

		using var inputStream = File.OpenRead(file);
		using var outputStream = File.Create(destFile);
		ReanimBinaryWriter.WriteToStream(ReanimXmlReader.ReadFromStream(inputStream), outputStream);
	}
}

{
	var files = Directory.GetFiles("particles", "*.xml", SearchOption.AllDirectories);
	if (!Directory.Exists(Path.Combine("compiled", "particles")))
		Directory.CreateDirectory(Path.Combine("compiled", "particles"));

	foreach (var file in files)
	{
		var destFile = Path.Combine("compiled", file + ".compiled");

		Console.WriteLine(file);

		using var inputStream = File.OpenRead(file);
		using var outputStream = File.Create(destFile);
		ParticlesBinaryWriter.WriteToStream(ParticlesXmlReader.ReadFromStream(inputStream), outputStream);
	}
}
