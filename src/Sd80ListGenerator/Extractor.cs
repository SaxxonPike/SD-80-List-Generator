namespace Sd80ListGenerator;

/// <summary>
/// Determines where in the editor executable the lists are located.
/// </summary>
public static class Extractor
{
    /// <summary>
    /// Bytes of the first patch in sequence. These bytes should be for the patch "Piano 1".
    /// </summary>
    private static readonly byte[] FirstPatch =
    {
        0x79, 0x00, 0x00, 0x50,
        0x69, 0x61, 0x6E, 0x6F,
        0x20, 0x31, 0x20, 0x20,
        0x20, 0x20, 0x20, 0x00
    };

    /// <summary>
    /// Bytes of the first waveform in sequence. The waveform list are stored in reverse. These bytes should be
    /// for the waveform called "BsGt Play Nz".
    /// </summary>
    private static readonly byte[] FirstWave =
    {
        0x42, 0x73, 0x47, 0x74,
        0x20, 0x50, 0x6C, 0x61,
        0x79, 0x20, 0x4E, 0x7A,
        0x00, 0x00, 0x00, 0x00
    };

    // Determines if the span matches the specified heuristic.
    private static bool Match(ReadOnlySpan<byte> buff, ReadOnlySpan<byte> heuristic)
    {
        for (var i = 0; i < buff.Length; i++)
        {
            if (buff[i] != heuristic[i])
                return false;
        }

        return true;
    }

    // Determines if the span contains any zeroes.
    private static bool AnyZeroes(ReadOnlySpan<byte> buff)
    {
        for (var i = 0; i < buff.Length; i++)
        {
            if (buff[i] == 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Try to extract patch and wave lists.
    /// </summary>
    public static ExtractResult Extract(Stream stream)
    {
        var buffer = new byte[16].AsSpan();
        var foundFirstPatch = false;
        var foundFirstWave = false;
        var inOffset = buffer.Length;
        var skipRead = true;
        var result = new ExtractResult();

        using var patchStream = new MemoryStream();
        using var waveStream = new MemoryStream();

        // In the event we don't even have enough bytes, bail.
        if (stream.Read(buffer) < buffer.Length)
            return result;

        // Main loop, slow method. But the code's small and so is the dataset, so it doesn't matter.
        while (true)
        {
            if (skipRead)
            {
                skipRead = false;
            }
            else
            {
                if (stream.Read(buffer[^1..]) < 1)
                    break;
                inOffset++;
            }

            // See if we match on the patch heuristic.
            if (!foundFirstPatch && Match(buffer, FirstPatch))
            {
                foundFirstPatch = true;
                result.PatchOffset = inOffset - buffer.Length;
                patchStream.Write(buffer);

                // Extract loop.
                while (true)
                {
                    if (stream.Read(buffer) < buffer.Length)
                        break;

                    if (AnyZeroes(buffer.Slice(0x3, 12)))
                        break;

                    patchStream.Write(buffer);
                    inOffset += buffer.Length;
                }

                result.PatchData = patchStream.ToArray();

                // Buffer already has the next bytes to search.
                skipRead = true;
                continue;
            }

            // See if we match on the wave heuristic.
            if (!foundFirstWave && Match(buffer, FirstWave))
            {
                foundFirstWave = true;
                result.WaveformOffset = inOffset - buffer.Length;
                waveStream.Write(buffer);

                // Extract loop.
                while (true)
                {
                    if (stream.Read(buffer) < buffer.Length)
                        break;

                    if (AnyZeroes(buffer[..12]))
                        break;

                    waveStream.Write(buffer);
                    inOffset += buffer.Length;
                }

                result.WaveformData = waveStream.ToArray();

                // Buffer already has the next bytes to search.
                skipRead = true;
                continue;
            }

            // Prepare the buffer for the next read.
            buffer[1..].CopyTo(buffer);
        }

        return result;
    }
}