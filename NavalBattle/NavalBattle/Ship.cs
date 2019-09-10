using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Direction
{
    Up, Down, Right, Left
}


public class Ship
{
    public Cell[] deck;
    public Cell bow;
    public Direction direction;
    public string type;
    public bool alive;

    public int HP
    {
        get
        {
            int hp = 0;
            foreach (var cell in deck)
            {
                if (cell.cellType == CellType.Live)
                {
                    hp++;
                }
            }
            return hp;
        }
    }

    public Ship(Cell[] deck, Cell bow, Direction direction, string type)
    {
        this.deck = deck;
        this.bow = bow;
        this.direction = direction;
        this.type = type;
        alive = true;
        foreach (var cell in deck)
        {
            cell.ship = this;
        }
    }
}

