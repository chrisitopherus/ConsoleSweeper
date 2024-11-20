using Minesweeper.Board;
using Minesweeper.Events;

namespace Minesweeper;

public class Game
{
    private GameConfiguration config;
    private GameBoard board;
    private GameCursor cursor;

    public event EventHandler<GameStartedEventArgs> GameStarted;
    public event EventHandler<GameWonEventArgs> GameWon;
    public event EventHandler<GameLossEventArgs> GameLoss;
    public event EventHandler<CursorMovedEventArgs> CursorMoved;
    public event EventHandler<CellsUpdatedEventArgs> CellsUpdated;

    public Game(GameConfiguration config)
    {
        this.config = config;
        this.board = new GameBoard(config);
        this.cursor = new GameCursor(this.board);
    }

    public GameBoard Board => this.board;
    public GameCursor Cursor => this.cursor;

    public void Start()
    {
        this.FireOnEvent(this.GameStarted, new GameStartedEventArgs());
    }

    /// <summary>
    /// Tries to reveal the cell beneath the cursor.
    /// </summary>
    public void TryReveal()
    {
        bool hasChanged = this.board.RevealCell(this.cursor.CurrentPosition);

        if (hasChanged)
        {
            this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs());
        }
    }

    /// <summary>
    /// Tries to mark the cell beneath the cursor.
    /// </summary>
    public void TryToggleMark()
    {
        bool hasChanged = this.board.ToggleCellMark(this.cursor.CurrentPosition);

        if (hasChanged)
        {
            this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs());
        }
    }

    /// <summary>
    /// Moves the cursor in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the cursor should move.</param>
    public void MoveCursor(CursorMoveDirection direction)
    {
        switch (direction)
        {
            case CursorMoveDirection.Left:
                this.cursor.MoveLeft();
                break;
            case CursorMoveDirection.Right:
                this.cursor.MoveRight();
                break;
            case CursorMoveDirection.Up:
                this.cursor.MoveUp();
                break;
            case CursorMoveDirection.Down:
                this.cursor.MoveDown();
                break;
        }

        this.FireOnEvent(this.CursorMoved, new CursorMovedEventArgs());
    }

    protected virtual void FireOnEvent<TEventArgs>(EventHandler<TEventArgs> eventHandler, TEventArgs e)
    {
        eventHandler?.Invoke(this, e);
    }
}
