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
    private EmptyTilesRevealStrategy emptyTilesRevealStrategy;
    private SurroundingTilesRevealStrategy surroundingTilesRevealStrategy;

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

    /// <summary>
    /// Is fired when the flag ammo changed/updated.
    /// </summary>
    public event EventHandler<FlagAmmoUpdatedEventArgs>? FlagAmmoUpdated;

    public Game(GameConfiguration config)
    {
        this.config = config;
        
        // set the flag ammo to the mine count -> only as much flags as mines
        this.flagAmmo = config.MineCount;
        this.board = new GameBoard(config);
        this.cursor = new GameCursor(this.board);
        this.emptyTilesRevealStrategy = new EmptyTilesRevealStrategy();
        this.surroundingTilesRevealStrategy = new SurroundingTilesRevealStrategy();
    }

    /// <summary>
    /// The state of the game.
    /// </summary>
    public GameState CurrentState
    {
        get
        {
            return this.state;
        }

        private set
        {
            this.PreviousState = this.state;
            this.state = value;
        }
    }
    public GameState PreviousState { get; private set; }

    /// <summary>
    /// The board of the game.
    /// </summary>
    public GameBoard Board => this.board;

    /// <summary>
    /// The cursor of the game.
    /// </summary>
    public GameCursor Cursor => this.cursor;

    /// <summary>
    /// The configuration of the game.
    /// </summary>
    public GameConfiguration Config => this.config;

    /// <summary>
    /// Gets or sets the amount of flags left to place.
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

            if (this.flagAmmo != value)
            {
                this.FireOnEvent(this.FlagAmmoUpdated, new FlagAmmoUpdatedEventArgs(value));
            }

            this.flagAmmo = value;
        }
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void Start()
    {
        this.board.Generate();
        this.CurrentState = GameState.Running;
        this.FireOnEvent(this.GameStarted, new GameStartedEventArgs(this.config));
    }

    /// <summary>
    /// Tries to reveal the cell beneath the cursor.
    /// </summary>
    public void TryReveal()
    {
        if (this.CurrentState != GameState.Running) return;

        CellInfo cellInfo = new CellInfo(this.board.GetCellAt(this.cursor.CurrentPosition), this.cursor.CurrentPosition);
        List<CellChangeInfo> updatedCells = [];
        if (cellInfo.Cell.IsRevealed)
        {
            // if its a mine or empty tile -> do nothing
            if (cellInfo.Cell.Type != CellType.Tile || cellInfo.Cell.IsEmpty) return;

            // use strategy to reveal surrounding
            updatedCells = this.surroundingTilesRevealStrategy.Reveal(this.board, cellInfo);
        }
        else if (cellInfo.Cell.IsEmpty)
        {
            // Use Empty Reveal Strategy
            updatedCells = this.emptyTilesRevealStrategy.Reveal(this.Board, cellInfo);
        }
        else
        {
            updatedCells.Add(new CellChangeInfo(cellInfo, cellInfo.Cell.IsMarked ? CellChangeType.UnmarkedAndRevealed : CellChangeType.Revealed));
            // otherwise reveal it
            cellInfo.Cell.Reveal();
        }

        // check if the flag ammo needs to be updated
        this.UpdateFlagAmmo(updatedCells);

        // send update event
        this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs(updatedCells));

        // check if game is lost
        IEnumerable<CellChangeInfo>? revealedMines = this.ExtractRevealedMines(updatedCells);
        if (revealedMines != null)
        {
            // State -> Loss
            this.CurrentState = GameState.Loss;
            this.FireOnEvent(this.GameLoss, new GameLossEventArgs());
        }

        // I dont know if its needed here
        // Check if all flags are placed -> could be a win
        if (this.FlagAmmo == 0 && this.CheckIfAllMinesAreFlagged())
        {
            this.CurrentState = GameState.Win;
            this.FireOnEvent(this.GameWon, new GameWonEventArgs());
        }
    }

    /// <summary>
    /// Tries to mark the cell beneath the cursor.
    /// </summary>
    public void TryToggleMark()
    {
        if (this.CurrentState != GameState.Running) return;

        CellInfo cellInfo = new CellInfo(this.board.GetCellAt(this.cursor.CurrentPosition), this.cursor.CurrentPosition);
        
        // if no ammo for marking an unmarked cell -> stop
        if (this.flagAmmo == 0 && !cellInfo.Cell.IsMarked) return;

        bool hasChanged = this.board.ToggleCellMark(cellInfo.Cell);

        if (hasChanged)
        {
            List<CellChangeInfo> updatedCells = [];
            updatedCells.Add(new CellChangeInfo(cellInfo, cellInfo.Cell.IsMarked ? CellChangeType.Marked : CellChangeType.Unmarked));

            // check if the flag ammo needs to be updated
            this.UpdateFlagAmmo(updatedCells);
            this.FireOnEvent(this.CellsUpdated, new CellsUpdatedEventArgs(updatedCells));

            // Check if all flags are placed -> could be a win
            if (this.FlagAmmo == 0 && this.CheckIfAllMinesAreFlagged())
            {
                this.CurrentState = GameState.Win;
                this.FireOnEvent(this.GameWon, new GameWonEventArgs());
            }
        }
    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void Restart()
    {
        if (this.CurrentState != GameState.Win && this.CurrentState != GameState.Loss) return;

        this.Reset();
        this.Start();
    }

    /// <summary>
    /// Moves the cursor in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the cursor should move.</param>
    public void MoveCursor(CursorMoveDirection direction)
    {
        if (this.CurrentState != GameState.Running) return;

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

    public void OpenMenuState()
    {
        this.CurrentState = GameState.Menu;
    }

    public void CloseMenuState()
    {
        this.CurrentState = this.PreviousState;
    }

    public void UpdateConfiguration(GameConfiguration newConfiguration)
    {
        this.Config.UpdateConfig(newConfiguration);
        this.Reset();
    }

    /// <summary>
    /// Tries to invoke the specified <see cref="EventHandler"/> with the specified <see cref="EventArgs"/>.
    /// </summary>
    /// <typeparam name="TEventArgs">Event arguments.</typeparam>
    /// <param name="eventHandler">The event handler to invoke.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void FireOnEvent<TEventArgs>(EventHandler<TEventArgs>? eventHandler, TEventArgs e)
    {
        eventHandler?.Invoke(this, e);
    }

    /// <summary>
    /// Resets the game.
    /// </summary>
    private void Reset()
    {
        this.board = new GameBoard(this.config);
        this.cursor = new GameCursor(this.board);
        this.FlagAmmo = this.config.MineCount;
    }

    /// <summary>
    /// Updates the flag ammo if neccessary.
    /// </summary>
    /// <param name="cellChanges">A list of cell changes.</param>
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
                    this.FlagAmmo -= 1;
                    break;
                default:
                    // other cases are not relevant for the flag ammo
                    break;
            }
        }
    }

    /// <summary>
    /// Extracts all revealed mines from a collection of changed cells.
    /// </summary>
    /// <param name="cellChangeInfos">The changed cells.</param>
    /// <returns>A list of all revealed mines or null.</returns>
    private List<CellChangeInfo>? ExtractRevealedMines(IEnumerable<CellChangeInfo> cellChangeInfos)
    {
        List<CellChangeInfo> revealedMines = [];

        foreach (CellChangeInfo cellChangeInfo in cellChangeInfos)
        {
            if (cellChangeInfo.Cell.Type == CellType.Mine)
            {
                revealedMines.Add(cellChangeInfo);
            }
        }

        return revealedMines.Count != 0 ? revealedMines : null;
    }

    /// <summary>
    /// Checks if all the mines are flagged.
    /// </summary>
    /// <returns>Whether all mines are flagged.</returns>
    private bool CheckIfAllMinesAreFlagged()
    {
        IEnumerable<ICellInfo> mines = this.board.GetFilteredCells(info => info.Cell.Type == CellType.Mine);
        foreach (CellInfo mineInfo in mines)
        {
            if (!mineInfo.Cell.IsMarked) return false;
        }

        return true;
    }
}
