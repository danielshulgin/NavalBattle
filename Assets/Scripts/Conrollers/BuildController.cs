using UnityEngine;
using NavalBattle;

public enum Turn
{
    FirstPlayer, SecondPlayer
}

public class BuildController : MonoBehaviour
{
    [HideInInspector] public Settings settings;

    GameObject[,] cellsVizual;
    public int shipTypeNumber;
    public bool inPrototype = false;
    public Direction direction = Direction.Right;

    public Map map;
    public bool active = true;
    Cell[] selectedDecks;

    public void ResetMap()
    {
        active = true;
        map = new Map(settings.mapWidth, settings.mapHeight);
        inPrototype = false;
        shipTypeNumber = 0;
        direction = Direction.Right;
        UpdateVizual(cellsVizual, map);
    }

    public void DeleteMap()
    {
        active = false;
        foreach (var go in cellsVizual)
        {
            Destroy(go);
        }
    }

    public void Start()
    {
        cellsVizual = new GameObject[settings.mapWidth, settings.mapHeight];
        for (int x = 0; x < settings.mapWidth; x++)
        {
            for (int y = 0; y < settings.mapHeight; y++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.GetComponent<Renderer>().material = settings.eptyMaterial;
                go.transform.position = new Vector3(x * settings.cellLength, y * settings.cellLength, 1f);
                cellsVizual[x, y] = go;
            }
        }
        ResetMap();
    }


    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(2) || inPrototype)
            {
                int x;
                int y;
                if (GetMapPosFromWorldPos(map, Vector2.zero, Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y))
                {
                    Cell[] deck = new Cell[settings.ships[shipTypeNumber].length];
                    if (selectedDecks != null)
                        foreach (var cell in selectedDecks)
                        {
                            cell.cellType = CellType.Empty;
                        }
                    if (map.CanPutShip(x, y, settings.ships[shipTypeNumber].length, direction, out deck))
                    {

                        selectedDecks = deck;
                        foreach (var cell in deck)
                        {
                            cell.cellType = CellType.Live;
                        }
                    }
                    UpdateVizual(cellsVizual, map);
                    inPrototype = true;
                }

            }
            else
            {
                if (selectedDecks != null)
                    foreach (var cell in selectedDecks)
                    {
                        cell.cellType = CellType.Empty;
                    }
            }
            if (Input.GetMouseButtonUp(2))
            {
                int x;
                int y;
                if (GetMapPosFromWorldPos(map, Vector2.zero, Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y))
                {
                    Cell[] deck = new Cell[settings.ships[shipTypeNumber].length];
                    if (selectedDecks != null)
                        foreach (var cell in selectedDecks)
                        {
                            cell.cellType = CellType.Empty;
                        }
                    if (map.CanPutShip(x, y, settings.ships[shipTypeNumber].length, direction, out deck))
                    {
                        foreach (var cell in deck)
                        {
                            cell.cellType = CellType.Live;
                        }
                        map.PutShip(deck, new Ship(deck, deck[0], direction, settings.ships[shipTypeNumber].name));
                    }
                    UpdateVizual(cellsVizual, map);
                }
                selectedDecks = null;
                inPrototype = false;
            }
        }
    }

    //TODO
    /// <param name="activePlayer">vizual owned by player whose turn is now</param>
    void UpdateVizual(GameObject[,] cellsVizual, Map map)
    {
        for (int x = 0; x < settings.mapWidth; x++)
        {
            for (int y = 0; y < settings.mapHeight; y++)
            {
                Material materialForCell = settings.eptyMaterial;
                switch (map.cells[x, y].cellType)
                {
                    case CellType.Blocked:
                        materialForCell = settings.blockedMaterial;
                        break;
                    case CellType.Empty:
                        materialForCell = settings.eptyMaterial;
                        break;
                    case CellType.Live:
                        materialForCell = settings.shipMaterial;
                        break;
                    case CellType.Dead:
                        materialForCell = settings.deadMaterial;
                        break;
                }
                cellsVizual[x, y].GetComponent<Renderer>().material = materialForCell;
            }
        }
    }

    public bool GetMapPosFromWorldPos(Map map, Vector2 mapstart/*TODO*/, Vector2 worldPosition, out int x, out int y)
    {
        if ((worldPosition.x >= mapstart.x && worldPosition.x < settings.cellLength * settings.mapWidth) &&
            (worldPosition.y >= mapstart.y && worldPosition.y < settings.cellLength * settings.mapHeight))
        {
            x = (int)((worldPosition.x - mapstart.x) / settings.cellLength + 0.5f);
            y = (int)((worldPosition.y - mapstart.y) / settings.cellLength + 0.5f);
            return true;
        }
        x = 0;
        y = 0;
        return false;
    }
}
