# SD-80-List-Generator

Extracts patch and waveform lists from the SD-80 editor.

### Run from Source

You need to have .NET Core SDK installed on your system, which will give you the .NET CLI.

Execute `dotnet run` in the `src/Sd80ListGenerator` folder. The output will appear in the console.

### Usage

`Sd80ListGenerator <path>`

- If you don't specify `path`, it will look for `Edirol/SD-80Editor/SD-80Editor.exe` in the `Program Files (x86)` folder, wherever your operating system has placed that.
- Output goes to the console; pipe it to somewhere else like a file if you want.

### Contributions

Make your changes and submit a pull request on Github for review.

### Bugs

File an issue on Github. Be specific about these three things: what you did, what you expected, what actually happened.
