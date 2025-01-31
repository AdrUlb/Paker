using PopLib.Particles;
using PopLib.Reanim;
using System.Text;

var files = Directory.GetFiles("reanim", "*.reanim", SearchOption.AllDirectories);
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

//ParticleXmlReader.ReadFromString(File.ReadAllText("particles/Award.xml"));

{
	using var fs = File.OpenRead("LanternShine.xml.compiled");
	var particles = ParticlesBinaryReader.ReadFromStream(fs);
	var sb = new StringBuilder();
	ParticlesXmlWriter.WriteToStringBuilder(particles, sb);
	File.WriteAllText("LanternShine.xml.decompiled", sb.ToString());
}

{
	using var fs = File.OpenRead("particles/LanternShine.xml");
	var particles = ParticlesXmlReader.ReadFromStream(fs);
	var sb = new StringBuilder();
	ParticlesXmlWriter.WriteToStringBuilder(particles, sb);
	File.WriteAllText("LanternShine.xml.decompiled2", sb.ToString());
}
