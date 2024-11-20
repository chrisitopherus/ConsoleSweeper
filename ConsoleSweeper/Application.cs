using Minesweeper;
using Minesweeper.Board;
using Minesweeper.Events;
using Minesweeper.Strategy;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSweeper;

public class Application
{
    private readonly Game game;
    public Application()
    {
        this.game = new Game(new GameConfiguration(8, 8, 4));
        this.Setup();
    }
    public void Run()
    {
        var randomInstance = new Random();
        var config = new GameConfiguration(8, 8, 4);
        this.game.Start();
        Console.OutputEncoding = Encoding.UTF8;
        
        IFieldGenerationStrategy generationStrategy = new SimpleRandomGenerationStrategy(config);
        GameCell[,] field = generationStrategy.Generate();

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                Console.Write(field[i, j]);
            }

            Console.WriteLine();
        }

        var defaultBg = Console.BackgroundColor;
        var defaultFg = Console.ForegroundColor;

        Console.WriteLine("┌───┬───┬───┬───┬───┐");

        Console.Write("│ ? │ ? │");
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(" 1 ");
        Console.BackgroundColor = defaultBg;
        Console.ForegroundColor = defaultFg;
        Console.WriteLine("│ * │ 2 │");

        Console.WriteLine("├───┼───┼───┼───┼───┤");

        Console.WriteLine("│ 1 │ ⚑ │ ? │ 2 │ ✪ │");

        Console.WriteLine("├───┼───┼───┼───┼───┤");

        Console.WriteLine("│ * │ 2 │   │ 3 │ 2 │");

        Console.WriteLine("└───┴───┴───┴───┴───┘");
        Console.ReadKey();
        Console.Clear();
        var tester = new LogicTestDrawer();

        var board = new GameBoard(config);
        for (int i = 0; i < board.GameField.GetLength(0); i++)
        {
            for (int j = 0; j < board.GameField.GetLength(1); j++)
            {
                Console.Write(board.GameField[i, j]);
            }

            Console.WriteLine();
        }

        Console.ReadKey();
        Console.Clear();
        tester.Draw(board);
        while (true)
        {
            int row = int.Parse(Console.ReadLine() ?? "0");
            int col = int.Parse(Console.ReadLine() ?? "0");
            Console.Clear();
            var pos = new BoardPosition(row, col);
            tester.Reveal(board, board.GetCellAt(pos), pos);
            tester.Draw(board);
        }

    
    }

    private void Setup()
    {
        this.game.GameStarted += this.OnGameStartedHandler;
        this.game.GameWon += this.OnGameWonHandler;
        this.game.GameLoss += this.OnGameLossHandler;
        this.game.CursorMoved += this.OnCursorMovedHandler;
        this.game.CellsUpdated += this.OnCellsUpdatedHandler;
    }

    private void OnGameStartedHandler(object? sender, GameStartedEventArgs e)
    {
        Console.WriteLine("Game started");
    }

    private void OnGameWonHandler(object? sender, GameWonEventArgs e)
    {
        Console.WriteLine("Game won.");
    }

    private void OnGameLossHandler(object? sender, GameLossEventArgs e)
    {
        Console.WriteLine("Game lost.");
    }

    private void OnCursorMovedHandler(object? sender, CursorMovedEventArgs e)
    {
        Console.WriteLine("Cursor moved.");
    }

    private void OnCellsUpdatedHandler(object? sender, CellsUpdatedEventArgs e)
    {
        Console.WriteLine("Cells updated.");
    }
}