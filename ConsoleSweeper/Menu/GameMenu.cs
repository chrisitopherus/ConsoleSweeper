using ConsoleSweeper.Menu.Events;
using ConsoleSweeper.Menu.Interfaces;
using ConsoleSweeper.Menu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Menu;

public class GameMenu
{
    private bool isOpen = false;
    private int index = 0;
    private List<IMenuItem> items = [];

    public event EventHandler<OnMenuOpenedEventArgs>? OnMenuOpened;
    public event EventHandler<OnMenuClosedEventArgs>? OnMenuClosed;
    public event EventHandler<OnMenuIndexChangedEventArgs>? OnMenuIndexChanged;
    public GameMenu()
    {
    }

    public bool IsOpen => this.isOpen;

    public int ItemCount => this.items.Count;

    public int Index => this.index;

    public IMenuItem? SelectedItem
    {
        get
        {
            if (this.ItemCount == 0)
            {
                return null;
            }

            return this.items[this.index];
        }
    }

    /// <summary>
    /// Used to "open" the menu.
    /// </summary>
    public void Open()
    {
        if (this.ItemCount == 0)
        {
            throw new InvalidOperationException("Menu needs at least 1 item.");
        }

        this.isOpen = true;

        this.FireOnEvent(this.OnMenuOpened, new OnMenuOpenedEventArgs());
    }

    /// <summary>
    /// Used to "close" the menu.
    /// </summary>
    public void Close()
    {
        this.index = 0;
        this.isOpen = false;
        this.FireOnEvent(this.OnMenuClosed, new OnMenuClosedEventArgs());
    }

    public GameMenu Add(IMenuItem menuItem)
    {
        this.items.Add(menuItem);
        return this;
    }

    public IMenuItem GetItemAtIndex(int index)
    {
        return this.items[index];
    }

    public IEnumerable<IMenuItem> GetItems()
    {
        for (int i = 0; i < this.ItemCount; i++)
        {
            yield return this.items[i];
        }
    }

    public IEnumerable<MenuItemInfo> GetItemsWithInfo()
    {
        for (int i = 0; i < this.ItemCount; i++)
        {
            yield return new MenuItemInfo(this.items[i], this.Index == i);
        }
    }

    public void NextItem()
    {
        if (this.ItemCount == 0)
        {
            throw new InvalidOperationException("Menu needs at least 1 item.");
        }

        this.ChangeIndex(1);
    }

    public void PrevItem()
    {
        if (this.ItemCount == 0)
        {
            throw new InvalidOperationException("Menu needs at least 1 item.");
        }

        this.ChangeIndex(-1);
    }

    public void ExecuteCurrentItem()
    {
        if (this.ItemCount == 0)
        {
            throw new InvalidOperationException("Menu needs at least 1 item.");
        }

        this.SelectedItem?.Execute();
    }

    protected virtual void FireOnEvent<TEventArgs>(EventHandler<TEventArgs>? eventHandler, TEventArgs e)
    {
        eventHandler?.Invoke(this, e);
    }

    private void ChangeIndex(int delta)
    {
        int prevIndex = this.index;

        this.index = (this.index + delta + this.ItemCount) % this.ItemCount;
        this.FireOnEvent(this.OnMenuIndexChanged, new OnMenuIndexChangedEventArgs(prevIndex, this.index));
    }
}
