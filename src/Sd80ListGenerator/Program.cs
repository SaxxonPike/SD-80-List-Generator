using Sd80ListGenerator;

Console.WriteLine("-- Sd80ListGenerator --");
Console.WriteLine();

var path = args.Length >= 1 ? args[0] : Locator.Locate();
if (path == default)
{
    Console.WriteLine("Was not able to find the SD-80 editor.\n" +
                      "If it was specified, it was not found.\n" +
                      "If it was not specified, it was not found in the default location.");
    return;
}

using var stream = File.OpenRead(path);
var data = Extractor.Extract(stream);

if (data.PatchOffset != default)
{
    Console.WriteLine($"Patches found at 0x{data.PatchOffset:X}");
    Console.WriteLine();
    Decoder.DecodePatchList(data.PatchData.Span, Console.Out);
    Console.WriteLine();
}

if (data.WaveformOffset != default)
{
    Console.WriteLine($"Waves found at 0x{data.WaveformOffset:X}");
    Console.WriteLine();
    Decoder.DecodeWaveformList(data.WaveformData.Span, Console.Out);
    Console.WriteLine();
}