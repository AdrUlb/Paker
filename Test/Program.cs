using PopLib.Particles;
using PopLib.Reanim;

/*
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
*/

{
	var filesXml = Directory.GetFiles("particles_xml", "*.xml", SearchOption.AllDirectories);
	var filesCompiled = Directory.GetFiles("particles_compiled", "*.xml.compiled", SearchOption.AllDirectories);
	//ReadOnlySpan<string> filesXml = ["particles_xml/PotatoMine.xml"];
	//ReadOnlySpan<string> filesCompiled = ["particles_compiled/PotatoMine.xml.compiled"];
	if (!Directory.Exists("generated/a"))
		Directory.CreateDirectory("generated/a");
	if (!Directory.Exists("generated/b"))
		Directory.CreateDirectory("generated/b");
	
	foreach (var file in filesCompiled)
	{
		var destFile = Path.Combine("generated", "a", file[19..^9]).ToLower();

		Console.WriteLine(file);

		using var inputStream = File.OpenRead(file);
		using var outputStream = File.Create(destFile);
		ParticlesXmlWriter.WriteToStream(ParticlesBinaryReader.ReadFromStream(inputStream), outputStream);
	}
	
	foreach (var file in filesXml)
	{
		var destFile = Path.Combine("generated", "b", file[14..]).ToLower();

		Console.WriteLine(file);

		using var inputStream = File.OpenRead(file);
		using var outputStream = File.Create(destFile);
		ParticlesXmlWriter.WriteToStream(ParticlesXmlReader.ReadFromStream(inputStream), outputStream);
	}
}

/*
using var inputStream = File.OpenRead("particles_xml/LanternShine.xml");
using var outputStream = File.Create("LanternShine.xml2");
ParticlesXmlWriter.WriteToStream(ParticlesXmlReader.ReadFromStream(inputStream), outputStream);
*/
