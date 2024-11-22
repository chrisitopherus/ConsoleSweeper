using Minesweeper.Board;
using Minesweeper.Events;
using Minesweeper.Strategy;

namespace Minesweeper;

public class Game
{
    private GameConfiguration config;
    private GameBoard board;
    private GameCursor cursor;
    private EmptyTilesRevealStrategy emptyTilesRevealStrategy = new EmptyTilesRevealStrategy();

    public event EventHandler<GameStartedEventArgs>? GameStarted;
    public event EventHandler<GameWonEventArgs>? GameWon;
    public event EventHandler<GameLossEventArgs>? GameLoss;
    public event EventHandler<CursorMovedEventArgs>? CursorMoved;
    public event EventHandler<CellsUpdatedEventArgs>? CellsUpdated;

    public Game(GameConfiguration config)
    {
        this.config = config;
        this.board = new GameBoard(config);
        this.cursor = new GameCursor(this.board);
    }

    public GameBoard Board => this.board;
    public GameCursor Cursor => this.cursor;

    public GameConfiguration Config => this.config;

    public void Start()
    {
        this.FireOnEvent(this.GameStarted, new GameStartedEventArgs(this.config));
    }

    /// <summary>
    /// Tries to reveal the cell beneath the cursor.
    /// </summary>
    public void TryReveal()
    {
        CellInfo cellInfo = new CellInfo(this.board.GetCellAt(this.cursor.CurrentPosition), this.cursor.CurrentPosition);
        List<CellInfo> updatedCells = [];
        if (cellInfo.Cell.IsRevealed)
        {
            if (cellInfo.Cell.Type != CellType.Tile || cellInfo.Cell.IsEmpty) return;

            // use strategy to reveal surrounding
            return;
        }

        if (cellInfo.Cell.IsEmpty)
        {
            // Use Empty Reveal Strategy
            updatedCells = this.emptyTilesRevealStrategy.Reveal(this.Board, cellInfo.Cell, cellInfo.Position);
        }
        else
        {
            // otherwise reveal it
            cellInfo.Cell.Reveal();
            updatedCells.Add(cellInfo);
        }

        this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs(updatedCells));
    }

    /// <summary>
    /// Tries to mark the cell beneath the cursor.
    /// </summary>
    public void TryToggleMark()
    {
        CellInfo cellInfo = new CellInfo(this.board.GetCellAt(this.cursor.CurrentPosition), this.cursor.CurrentPosition);
        bool hasChanged = this.board.ToggleCellMark(cellInfo.Cell);

        if (hasChanged)
        {
            List<CellInfo> updatedCells = [];
            updatedCells.Add(cellInfo);
            this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs(updatedCells));
        }
    }

    /// <summary>
    /// Moves the cursor in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the cursor should move.</param>
    public void MoveCursor(CursorMoveDirection direction)
    {
        BoardPosition previousPosition = this.cursor.CurrentPosition;
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

        this.FireOnEvent(this.CursorMoved, new CursorMovedEventArgs(previousPosition, this.cursor.CurrentPosition));
    }

    protected virtual void FireOnEvent<TEventArgs>(EventHandler<TEventArgs>? eventHandler, TEventArgs e)
    {
        eventHandler?.Invoke(this, e);
    }
}
