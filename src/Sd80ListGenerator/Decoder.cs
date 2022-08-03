using System.Text;

namespace Sd80ListGenerator;

public static class Decoder
{
    private static string FixQuotes(string s)
    {
        return s.Replace("\"", "\"\"");
    }
    
    public static void DecodeWaveformList(ReadOnlySpan<byte> bytes, TextWriter writer)
    {
        writer.WriteLine("Index,Name");
        for (var i = (bytes.Length - 1) / 16 * 16; i >= 0; i -= 16)
        {
            var record = bytes.Slice(i, 16);
            var idx = ((bytes.Length - i) / 16) - 1;
            var name = Encoding.ASCII.GetString(record[..12]).Trim();
            writer.WriteLine($"{idx},{FixQuotes(name)}");
        }
    }

    public static void DecodePatchList(ReadOnlySpan<byte> bytes, TextWriter writer)
    {
        writer.WriteLine("Index,MSB,LSB,PRG,Name");
        for (var i = 0; i < bytes.Length; i += 16)
        {
            var record = bytes.Slice(i, 16);
            var idx = i / 16 + 1;
            var msb = record[0];
            var lsb = record[1];
            var prg = record[2] + 1;
            var name = Encoding.ASCII.GetString(record.Slice(3, 12)).Trim();
            writer.WriteLine($"{idx},{msb},{lsb},{prg},{FixQuotes(name)}");
        }
    }
}