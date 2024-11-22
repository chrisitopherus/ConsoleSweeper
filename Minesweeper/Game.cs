using Minesweeper.Board;
using Minesweeper.Cells;
using Minesweeper.Events;
using Minesweeper.Strategy;

namespace Minesweeper;

public class Game
{
    private GameState state = GameState.NotStarted;
    private GameConfiguration config;
    private GameBoard board;
    private GameCursor cursor;
    private EmptyTilesRevealStrategy emptyTilesRevealStrategy = new EmptyTilesRevealStrategy();

    /// <summary>
    /// Specifies how many flags are left to play.
    /// </summary>
    private int flagAmmo;

    /// <summary>
    /// Is fired when the game started.
    /// </summary>
    public event EventHandler<GameStartedEventArgs>? GameStarted;

    /// <summary>
    /// Is fired when the game is won.
    /// </summary>
    public event EventHandler<GameWonEventArgs>? GameWon;

    /// <summary>
    /// Is fired when the game is lost.
    /// </summary>
    public event EventHandler<GameLossEventArgs>? GameLoss;

    /// <summary>
    /// Is fired when the cursor was moved.
    /// </summary>
    public event EventHandler<CursorMovedEventArgs>? CursorMoved;

    /// <summary>
    /// Is fired when cells have updated.
    /// </summary>
    public event EventHandler<CellsUpdatedEventArgs>? CellsUpdated;

    public Game(GameConfiguration config)
    {
        this.config = config;
        
        // set the flag ammo to the mine count -> only as much flags as mines
        this.flagAmmo = config.MineCount;
        this.board = new GameBoard(config);
        this.cursor = new GameCursor(this.board);
    }

    public GameBoard Board => this.board;
    public GameCursor Cursor => this.cursor;

    public GameConfiguration Config => this.config;

    /// <summary>
    /// Gets the amount of flags left to place.
    /// </summary>
    public int FlagAmmo
    {
        get
        {
            return this.flagAmmo;
        }

        private set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.FlagAmmo), "The flag ammo can not be less than 0.");
            }

            // send event if flag ammo changed - TODO

            this.flagAmmo = value;
        }
    }

    public void Start()
    {
        this.state = GameState.Running;
        this.FireOnEvent(this.GameStarted, new GameStartedEventArgs(this.config));
    }

    /// <summary>
    /// Tries to reveal the cell beneath the cursor.
    /// </summary>
    public void TryReveal()
    {
        if (this.state != GameState.Running) return;

        CellInfo cellInfo = new CellInfo(this.board.GetCellAt(this.cursor.CurrentPosition), this.cursor.CurrentPosition);
        List<CellChangeInfo> updatedCells = [];
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
            updatedCells.Add(new CellChangeInfo(cellInfo, cellInfo.Cell.IsMarked ? CellChangeType.UnmarkedAndRevealed : CellChangeType.Revealed));
            // otherwise reveal it
            cellInfo.Cell.Reveal();
        }

        // check if the flag ammo needs to be updated
        this.UpdateFlagAmmo(updatedCells);
        this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs(updatedCells));
    }

    /// <summary>
    /// Tries to mark the cell beneath the cursor.
    /// </summary>
    public void TryToggleMark()
    {
        if (this.state != GameState.Running) return;

        CellInfo cellInfo = new CellInfo(this.board.GetCellAt(this.cursor.CurrentPosition), this.cursor.CurrentPosition);
        bool hasChanged = this.board.ToggleCellMark(cellInfo.Cell);

        if (hasChanged)
        {
            List<CellChangeInfo> updatedCells = [];
            updatedCells.Add(new CellChangeInfo(cellInfo, cellInfo.Cell.IsMarked ? CellChangeType.Marked : CellChangeType.Unmarked));

            // check if the flag ammo needs to be updated
            this.UpdateFlagAmmo(updatedCells);
            this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs(updatedCells));
        }
    }

    /// <summary>
    /// Moves the cursor in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the cursor should move.</param>
    public void MoveCursor(CursorMoveDirection direction)
    {
        if (this.state != GameState.Running) return;

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

    private void UpdateFlagAmmo(List<CellChangeInfo> cellChanges)
    {
        foreach (CellChangeInfo changeInfo in cellChanges)
        {
            switch (changeInfo.ChangeType)
            {
                case CellChangeType.Unmarked:
                case CellChangeType.UnmarkedAndRevealed:
                    this.FlagAmmo += 1;
                    break;
                case CellChangeType.Marked:
                    this.flagAmmo -= 1;
                    break;
                default:
                    // other case are not relevant for the flag ammo
                    break;
            }
        }
    }
}
