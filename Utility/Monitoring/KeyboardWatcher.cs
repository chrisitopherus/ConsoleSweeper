namespace Utility.Monitoring;

using System;
using System.Threading;

public class KeyboardWatcher
{
    private CancellationTokenSource tokenSource;

    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    public KeyboardWatcher()
    {
        this.tokenSource = new CancellationTokenSource();
    }

    public CancellationTokenSource TokenSource
    {
        get
        {
            return this.tokenSource;
        }

        private set
        {
            this.tokenSource = value ?? throw new ArgumentNullException(nameof(TokenSource), "The Tokensource must not be null.");
        }
    }

    protected virtual void FireOnKeyPressed(KeyPressedEventArgs e)
    {
        this.KeyPressed?.Invoke(this, e);
    }

    public void Start()
    {
        this.TokenSource = new CancellationTokenSource();
        Task.Run(() => WatchKeysAsync(this.TokenSource.Token));
    }

    public void Stop()
    {
        if (this.TokenSource == null)
        {
            throw new InvalidOperationException("Cant stop before starting.");
        }

        this.TokenSource.Cancel();
    }

    /// <summary>
    /// Watches for key inputs and fires for each input an event.
    /// </summary>
    /// <param name="token">The cancellation token to stop it.</param>
    /// <returns>The completed task.</returns>
    private async Task WatchKeysAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                this.FireOnKeyPressed(new KeyPressedEventArgs(key));
            }

            await Task.Delay(10, cancellationToken);
        }
    }
}