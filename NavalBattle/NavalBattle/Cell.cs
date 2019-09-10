namespace NavalBattle
{
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
}

