using ConsoleSweeper.Menu;
using ConsoleSweeper.Menu.Interfaces;
using ConsoleSweeper.Menu.Util;
using ConsoleSweeper.Renderer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Cmd;

namespace ConsoleSweeper.Renderer.Menu;

public class GameMenuRenderer
{
    private GameMenu menu;
    private ConsolePosition origin;
    private ConsoleSettings renderSettings;

    public GameMenuRenderer(GameMenu menu, ConsolePosition origin, ConsoleSettings renderSettings)
    {
        this.menu = menu;
        this.origin = origin;
        this.renderSettings = renderSettings;
    }
    public void Render()
    {
        this.renderSettings.ApplyColors();
        Console.Clear();
        Console.SetCursorPosition(origin.X, origin.Y);
        foreach (MenuItemInfo info in this.menu.GetItemsWithInfo())
        {
            if (info.IsSelected)
            {
                this.ApplyColorsForSelectedItem();
            }
            else if (info.Item.IsActive)
            {
                this.renderSettings.ApplyColors();
            }
            else
            {
                this.ApplyInactiveColors();
            }

            Console.WriteLine(info.Item.Label);
            Console.CursorLeft = this.origin.X;
        }
    }

    public void Unrender()
    {
        Console.SetCursorPosition(origin.X, origin.Y);
        this.renderSettings.ApplyColors();
        foreach (IMenuItem item in this.menu.GetItems())
        {
            Console.WriteLine(new string(' ', item.Label.Length));
            Console.CursorLeft = this.origin.X;
        }
    }

    public void RenderMenuUpdate(int prevIndex, int newIndex)
    {
        Console.SetCursorPosition(origin.X, origin.Y + prevIndex);
        IMenuItem prevItem = this.menu.GetItemAtIndex(prevIndex);
        if (prevItem.IsActive)
        {
            this.renderSettings.ApplyColors();
        }
        else
        {
            this.ApplyInactiveColors();
        }
        Console.Write(prevItem.Label);
        Console.SetCursorPosition(origin.X, origin.Y + newIndex);
        this.ApplyColorsForSelectedItem();
        IMenuItem newItem = this.menu.GetItemAtIndex(newIndex);
        Console.Write(newItem.Label);
    }

    private void ApplyColorsForSelectedItem()
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Yellow;
    }

    private void ApplyInactiveColors()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Gray;
    }
}
