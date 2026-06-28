# QuickMath CLI

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](LICENSE)
[![Windows](https://img.shields.io/badge/Windows-supported-0078D6?logo=windows)](build.bat)
[![Linux](https://img.shields.io/badge/Linux-supported-E95420?logo=linux)](build.bat)
[![macOS](https://img.shields.io/badge/macOS-supported-000000?logo=apple)](build.bat)

A fast-paced terminal math game built in C# with [Spectre.Console](https://spectreconsole.net/). Challenge your arithmetic skills across addition, subtraction, and multiplication with multiple game modes and difficulty levels.

![QuickMath CLI screenshot](docs/screenshot.svg)

## Features

- **Multiple Game Modes**
  - **Standard** &mdash; 20 questions, 3 lives
  - **Infinite** &mdash; endless practice, 3 lives
  - **Timed** &mdash; as many questions as you can answer in 60 seconds
- **Operations** &mdash; choose any combination of addition, subtraction, and multiplication
- **Difficulty Levels** &mdash; Super Easy (1&ndash;10) through Expert (1&ndash;500), plus a Custom range
- **Live Stats** &mdash; real-time score, streak, lives, accuracy, and countdown timer
- **Colorful Terminal UI** &mdash; powered by Spectre.Console with figlet headers and styled panels
- **Cross-Platform** &mdash; pre-built binaries for Windows, Linux, and macOS

## Quick Start

1. **Download** the latest binary for your platform from the [Releases](https://github.com/pikmintea/QuickMath-CLI/releases) page.
2. **Run** the executable:
   ```shell
   # Windows
   QuickMathCLI.exe

   # Linux / macOS
   ./QuickMathCLI_Linux_x64
   ./QuickMathCLI_osx-x64
   ```

No dependencies required &mdash; binaries are self-contained.

## Building from Source

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later

### Clone & Build

```shell
git clone https://github.com/pikmintea/QuickMath-CLI.git
cd QuickMath-CLI
dotnet publish src/QuickMathCLI.csproj -r win-x64 --self-contained true -c Release
```

Or use the included build script for all three platforms:

```shell
.\build.bat
```

Outputs go to the `build/` directory:
```
build/
├── win-x64/    QuickMathCLI.exe
├── linux-x64/  QuickMathCLI_Linux_x64
└── osx-x64/    QuickMathCLI_osx-x64
```

## How to Play

1. Launch the game and select a **game mode**.
2. Choose which **operations** you want to practice (space to toggle).
3. Pick a **difficulty** level or set a custom number range.
4. Answer each question by typing the number and pressing **Enter**.
5. Press **Escape** at any time to quit.

## License

This project is licensed under the **GNU Affero General Public License v3.0**. See [LICENSE](LICENSE) for details.
