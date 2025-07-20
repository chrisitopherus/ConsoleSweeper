# ConsoleSweeper

ConsoleSweeper is a fully playable implementation of the classic Minesweeper game that runs entirely in your terminal. It is written in C# and targets **.NET 8**.

## Motivation

The project started as an exercise in building a console application while experimenting with event‑driven input and simple rendering techniques. It shows how a complete game can be implemented using only the standard library.

## Features

- Adjustable board size and mine count when starting a new game.
- Menu with `Resume`, `New Game` and `Exit` options.
- Keyboard controls via arrow keys or `WASD` for movement, `Space` to reveal, `F`/`M` to flag, `R` to restart and `Esc` for the menu.
- ASCII graphics with sprites defined in the renderer.
- Display of remaining flags and clear win/loss detection.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/)

## Build and Run

Clone the repository and run the game from the root directory:

```bash
dotnet run --project ConsoleSweeper
```

When starting a new game you will be prompted for the number of rows, columns and mines.

## Project Structure

- `ConsoleSweeper/` – console application, controller and renderer.
- `Minesweeper/` – core game logic and algorithms.
- `Utility/` – helper library for console settings and keyboard watching.

## License

ConsoleSweeper is released under the [MIT License](LICENSE).
