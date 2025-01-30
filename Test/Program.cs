using PopLib.Reanim;

{

	using var compiled = File.OpenRead("Blover.reanim.compiled");
	var reanim = ReanimBinaryReader.ReadFromStream(compiled);

	using var decompiled = File.OpenWrite("Blover.reanim.decompiled");
	ReanimXmlWriter.WriteToStream(reanim, decompiled);
}

{

	using var decompiled = File.OpenRead("Blover.reanim.decompiled");
	var reanim = ReanimXmlReader.ReadFromStream(decompiled);

	using var recompiled = File.OpenWrite("Blover.reanim.recompiled");
	ReanimBinaryWriter.WriteToStream(reanim, recompiled);
}

{

	using var compiled = File.OpenRead("Blover.reanim.recompiled");
	var reanim = ReanimBinaryReader.ReadFromStream(compiled);

	using var decompiled = File.OpenWrite("Blover.reanim.redecompiled");
	ReanimXmlWriter.WriteToStream(reanim, decompiled);
}
