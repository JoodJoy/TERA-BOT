

namespace tbp
{
  internal class cTimer
  {
    public int duration;
    public bool delayed;

    public void tick()
    {
      if (this.duration > 0)
      {
        --this.duration;
      }
      else
      {
        if (this.duration != 0)
          return;
        this.delayed = false;
      }
    }

    public void delay(int dur)
    {
      if (this.duration != 0)
        return;
      this.duration = dur;
      this.delayed = true;
    }
  }
}
