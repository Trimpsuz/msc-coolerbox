# Cooler Box

<img align="right" src="https://img.trimpsuz.dev/i/o88b9.png" alt="icon" width=128 />

Introduces two new cooler boxes to Teimo's store. Now you can keep your food and drinks cool while on the go!

The cooler box is half as effective as the regular fridge while closed, and slightly over twice as effective as nothing while open.

## Requirements
- [My Summer Car](https://store.steampowered.com/app/516750)
- [MSCLoader](https://github.com/piotrulos/MSCModLoader)
- [FridgeAPI](https://github.com/maceeikodev/FridgeAPI)

## Installation
### Using prebuilt binary
1. Download the mod from the releases page.
2. Paste the mod file into your MSCLoader mods folder (e.g. C:\SteamApps\common\My Summer Car\Mods).
3. Download FridgeAPI from the [github repository](https://github.com/maceeikodev/FridgeAPI) and paste it into your mods folder if you have not done so already.
4. Start the game and you're all set!

### Building from source
### Prerequisites
- Visual Studio 2022 (or compatible IDE)
- .NET Framework 3.5
### Building
1. Clone or download the mod's source code from this repository.
2. Open the solution file in Visual Studio.
3. Once the project is loaded, ensure that the necessary references are correctly configured. You may need to adjust the reference paths if the game or dependencies are installed in different locations.
4. Select your desired build configuration (e.g. Release) and platform (e.g. AnyCPU).
5. Build the project by selecting Build > Build Solution.
6. After the build process completes successfully, the compiled mod file (Cooler Box.dll) will be generated in the specified output directory (e.g. Cooler Box\bin\Release).
7. Copy the generated DLL file (Cooler Box.dll) from the output directory to your My Summer Car mods folder. The provided Post-Build Event will automate this process, but the path may not be correct.
8. Download (or build) FridgeAPI from the [github repository](https://github.com/maceeikodev/FridgeAPI) and paste it into your mods folder if you have not done so already.
9. Start the game and you're all set!

## License
This mod is distributed under the GNU General Public License v3. For more information, refer to the [LICENSE](LICENSE.md) file.