namespace Learning.eventArgs
{
  public class TickerEventArgs : EventArgs
  {
    public TickerEventArgs(TimeOnly time)
    {
      Time = time;
    }

    public TimeOnly Time { get; }
  }
}
