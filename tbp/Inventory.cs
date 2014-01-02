

using System.Drawing;

namespace tbp
{
  public class Inventory
  {
    public Point[,] pointArray = new Point[8, 13];
    private Point basePoint = new Point(815, 180);

    public Inventory()
    {
      Point point = new Point(this.basePoint.X, this.basePoint.Y);
      for (int index1 = 0; index1 < 13; ++index1)
      {
        point.Y += 27;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (index2 == 0)
            point.X = this.basePoint.X;
          else
            point.X += 27;
          this.pointArray[index2, index1] = point;
        }
      }
    }
  }
}
