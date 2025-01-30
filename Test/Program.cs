using PopLib.Reanim;


using var fs = File.OpenRead("Blover.reanim.compiled");
var reanim = ReanimBinaryReader.ReadFromStream(fs);

using var fs2 = File.OpenWrite("Blover.reanim.decompiled");
ReanimXmlWriter.WriteToStream(reanim, fs2);
