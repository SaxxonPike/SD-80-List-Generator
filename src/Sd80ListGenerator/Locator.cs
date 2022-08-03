namespace Sd80ListGenerator;

/// <summary>
/// Locates the editor executable. Only written for Windows at this time.
/// </summary>
public static class Locator
{
    /// <summary>
    /// Get the path of the editor. Null if it couldn't be found.
    /// </summary>
    public static string? Locate()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "EDIROL",
            "SD-80Editor",
            "SD-80Editor.exe");

        return File.Exists(path) ? path : null;
    }
}