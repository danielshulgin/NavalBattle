using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Map
{
    public Cell[,] cells;
    private readonly int width;
    private readonly int height;
    public Player owner;
    public event Action<Player> GameOver = (p) => { };
    public List<Ship> ships = new List<Ship>();

    public Map(Settings settings)
    {
        width = settings.mapWidth;
        height = settings.mapHeight;
        cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell(x, y);
            }
        }
    }

    public List<Cell> GetNearestCells(int x, int y, int radius)
    {
        var result = new List<Cell>();
        for (int Xi = x - radius; Xi <= x + radius; Xi++)
        {
            for (int Yi = y - radius; Yi <= y + radius; Yi++)
            {
                if (!(Xi == x && Yi == y) && !OutOfBounds(Xi, Yi))
                {
                    result.Add(cells[Xi, Yi]);
                }
            }
        }
        return result;
    }

    public void PutShip(Cell[] deck, Ship ship)
    {
        foreach (var cell in deck)
        {
            foreach (var neighbour in GetNearestCells(cell.x, cell.y, 1))
            {
                if (neighbour.cellType != CellType.Live)
                {
                    neighbour.cellType = CellType.Blocked;
                }
            }
        }
        ships.Add(ship);
    }

    public bool Damage(int x, int y)
    {
        if (cells[x, y].cellType == CellType.Empty)
        {
            cells[x, y].cellType = CellType.Miss;
            return true;
        }
        else if(cells[x, y].cellType == CellType.Live)
        {
            cells[x, y].cellType = CellType.Dead;
            if (cells[x, y].ship.HP == 0)
            {
                cells[x, y].ship.alive = false;
                bool existLiveShips = false;
                foreach (var ship in ships)
                {
                    if (ship.HP > 0)
                    {
                        existLiveShips = true;
                    }
                }
                if (existLiveShips == false)
                {
                    GameOver(owner);
                }
            }
            return true;
        }
        return false;
    }

    public bool CanPutShip(int x, int y, ShipSettings shipSettings, Direction direction, out Cell[] deck)
    {
        deck = new Cell[shipSettings.length];
        switch (direction)
        {
            case Direction.Up:
                for (int i = 0; i < shipSettings.length; i++)
                {
                    int Xi = x;
                    int Yi = y + i;
                    if (!CanPutCell(Xi, Yi))
                    {
                        return false;
                    }
                    deck[i] = cells[Xi, Yi];
                }
                break;
            case Direction.Down:
                for (int i = 0; i < shipSettings.length; i++)
                {
                    int Xi = x;
                    int Yi = y - i;
                    if (!CanPutCell(Xi, Yi))
                    {
                        return false;
                    }
                    deck[i] = cells[Xi, Yi];
                }
                break;
            case Direction.Right:
                for (int i = 0; i < shipSettings.length; i++)
                {
                    int Xi = x + i;
                    int Yi = y;
                    if (!CanPutCell(Xi, Yi))
                    {
                        return false;
                    }
                    deck[i] = cells[Xi, Yi];
                }
                break;
            case Direction.Left:
                for (int i = 0; i < shipSettings.length; i++)
                {
                    int Xi = x - i;
                    int Yi = y;
                    if (!CanPutCell(Xi, Yi))
                    {
                        return false;
                    }
                    deck[i] = cells[Xi, Yi];
                }
                break;
        }
        return true;
    }

    public bool CanPutCell(int x, int y)
    {
        if (OutOfBounds(x, y))
        {
            return false;
        }
        var cellsToCheck = GetNearestCells(x, y, 1);
        cellsToCheck.Add(cells[x, y]);
        foreach (var cell in cellsToCheck)
        {
            if (cell.cellType == CellType.Live)
            {
                return false;
            }
        }
        return true;
    }

    private bool OutOfBounds(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return false;
        }
        return true;
    }
}

