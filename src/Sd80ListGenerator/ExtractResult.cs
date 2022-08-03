namespace Sd80ListGenerator;

/// <summary>
/// Holds the heuristic result. This says where in the file this generator thinks there's the right data.
/// </summary>
public class ExtractResult
{
    /// <summary>
    /// Offset, in bytes, of the waveform list.
    /// </summary>
    public int? WaveformOffset { get; set; }
    
    /// <summary>
    /// Size, in bytes, of the waveform list.
    /// </summary>
    public Memory<byte> WaveformData { get; set; }
    
    /// <summary>
    /// Offset, in bytes, of the patch list.
    /// </summary>
    public int? PatchOffset { get; set; }
    
    /// <summary>
    /// Size, in bytes, of the patch list.
    /// </summary>
    public Memory<byte> PatchData { get; set; }
}