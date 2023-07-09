## Community Script Hook RDR2 .NET V2 | [![Downloads](https://raster.shields.io/github/downloads/Halen84/ScriptHookRDR2DotNet/total.svg?style=for-the-badge)](https://github.com/Halen84/ScriptHookRDR2DotNet/releases)

## ScriptHookRDR2DotNet V2

An improved version of [Saltyq's](https://github.com/Saltyq) [ScriptHookRDR2DotNet](https://github.com/Saltyq/ScriptHookRDR2DotNet) implementation.

All credits go to [Saltyq](https://github.com/Saltyq) for the original [ScriptHookRDR2DotNet](https://github.com/Saltyq/ScriptHookRDR2DotNet) implementation.

**Note: Older C# scripts may not be compatible with this version and likely needs to be updated.**

## About

This is an *ASI plugin for Red Dead Redemption 2*, ported from [**ScriptHookVDotNet**](https://github.com/crosire/scripthookvdotnet/), based on the C++ ScriptHook by Alexander Blade, which allows running scripts written in any .NET language in-game.

The issues page should be primarily used for bug reports and focused enhancement ideas. Questions related to RDR2 scripting in general are better off in [Discussions page](https://github.com/Halen84/ScriptHookRDR2DotNet-V2/discussions) or forums dedicated to this purpose, like [rdr2mods.com](https://www.rdr2mods.com/)

## Requirements

* [C++ ScriptHookRDR2 by Alexander Blade](http://www.dev-c.com/rdr2/scripthookrdr2/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Visual C++ Redistributable for Visual Studio 2019 x64](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)

## Downloads

Pre-built binaries can be found on the [releases](https://github.com/Halen84/ScriptHookRDR2DotNet-V2/releases) page.
You need to use the ASI file and the DLL files for APIs in an archive of the same version as internal structure can be changed without notices.

## Contributing

You'll need Visual Studio 2019 or higher to open the project file and the [Script Hook RDR2 SDK](http://dev-c.com/rdr2/scripthookrdr2/) extracted into "[/sdk](/sdk)".

Any contributions to the project are welcomed, it's recommended to use GitHub's [pull request](https://help.github.com/articles/using-pull-requests/) system.

## License

All the source code except for the Vector, Matrix and Quaternion classes, which are licensed separately, is licensed under the conditions of the [zlib license](LICENSE.txt).

## Credits
* Saltyq - Original ScriptHookRDR2DotNet creator
* crosire & kagikn - [ScriptHookVDotNet](https://github.com/scripthookvdotnet/scripthookvdotnet)
