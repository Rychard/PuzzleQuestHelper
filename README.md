# PuzzleQuestHelper
Just a simple (unfinished) tool for finding possible moves to the primary puzzles in [Puzzle Quest 2](https://en.wikipedia.org/wiki/Puzzle_Quest_2)

**Note:** This tool is far from perfect.  It works semi-reliably, so long as the following assumptions can be made:
- The game's executable is named `PuzzleQuest2.exe`
- The game is running in `Windowed` mode.
- The game's resolution is set to `1600x900`. 
  - The desktop resolution is `1920x1080`.
  - Sidenote: It's probably fine so long as the desktop resolution is higher than the game's.
  - For the sake of being thorough, I have two monitors that each run at `1920x1080`.  I doubt this matters, but still.
- A puzzle is active in the game-window.
- The active puzzle is of the `8x8` variety.

The constructor for the [`GameBoard.cs`](src/PuzzleQuestHelper.Logic/GameBoard.cs#L34-L40) class accepts parameters indicating the size of the game board, but it's currently [hard-coded](src/PuzzleQuestHelper.PresentationConsole/Program.cs#L15) to work for `8x8` puzzles.  I haven't tested this with puzzles of different sizes.

The mini-game puzzles have a different sized game board, but I didn't implement any way of detecting or handling these.  The [`GameBoardBitmapMapper`](src/PuzzleQuestHelper.Logic/GameBoardBitmapMapper.cs) class *should* work if given different parameters (notably a bitmap of the appropriate size, as well as the appropriate size of the grid), but again, I haven't tested it on anything other than the standard `8x8` puzzle grid.  Getting a bitmap of any arbitrary size is trivial using the [`WindowHelper.GetGameWindow(String, Rectangle)`](src/PuzzleQuestHelper.Logic/WindowHelper.cs#L38-L51) method.

## Building

1. Clone the repository
2. Open the [`src/PuzzleQuestHelper.sln`](src/PuzzleQuestHelper.sln) file in Visual Studio (it was written using VS2013)
3. Simply build the solution.  It's just a console application, and I'm not doing anything fancy.