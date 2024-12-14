using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSweeper.Interfaces;
using ConsoleSweeper.Menu.Interfaces;
using Minesweeper;
using Utility.Cmd;

namespace ConsoleSweeper.Menu;

public class NewGameMenuItem : IMenuItem
{
    private readonly IGameStartable subject;
    private ConsoleSettings renderSettings;
    public NewGameMenuItem(IGameStartable subject, ConsoleSettings renderSettings)
    {
        this.subject = subject;
        this.renderSettings = renderSettings;
    }
    public string Label => "New Game";

    public bool IsActive => true;

    public void Execute()
    {
        this.renderSettings.ApplyColors();
        this.subject.StartGame(this.AskUserForConfiguration());
    }

    private GameConfiguration AskUserForConfiguration()
    {
        Console.Clear();
        int rows = this.PromptInt("Rows: ");
        int cols = this.PromptInt("Cols: ");
        int mines = this.PromptInt("Mines: ");

        return new GameConfiguration(rows, cols, mines);
        
    }

    private int PromptInt(string message)
    {
        int value = 0;
        bool isValid = false;
        while (!isValid)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out value) && value > 0)
            {
                isValid = true;
            }
            else
            {
                Console.Write("\n Bitte eine ganze Zahl > 0.");
                isValid = false;
            }

            Console.Clear();
        }

        return value;
    }
}
