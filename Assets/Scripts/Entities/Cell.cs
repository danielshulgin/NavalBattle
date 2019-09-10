using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum CellType
{
    Blocked, Empty, Live, Dead, Miss
}

public class Cell
{
    public Ship ship;
    public CellType cellType;
    public int x;
    public int y;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        cellType = CellType.Empty;
    }
}

