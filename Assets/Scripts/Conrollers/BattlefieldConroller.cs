using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Mode
{
    FirstPlayerBuild, SecondPlayerBuild, FirstPlayerTurn, SecondPlayerTurn
}

public class BattlefieldConroller : MonoBehaviour
{
    [HideInInspector] public Settings settings;
    public BuildController buildController;

    public GameObject[,] firstPlayerCellsVizual;
    public GameObject[,] secondPlayerCellsVizual;

    public GameController gameController;

    public Map firstMap;
    public Map secondMap;
    public GameObject firstMapStart;
    public GameObject secondMapStart;

    public bool fired = false;
    public bool active = false;

    public void InstantiateBattleField()
    {
        firstPlayerCellsVizual = new GameObject[settings.mapWidth, settings.mapHeight];
        for (int x = 0; x < settings.mapWidth; x++)
        {
            for (int y = 0; y < settings.mapHeight; y++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.GetComponent<Renderer>().material = settings.eptyMaterial;
                go.transform.position = new Vector3(x * settings.cellLength, y * settings.cellLength, 1f) + firstMapStart.transform.position;
                firstPlayerCellsVizual[x, y] = go;
            }
        }
        secondPlayerCellsVizual = new GameObject[settings.mapWidth, settings.mapHeight];
        for (int x = 0; x < settings.mapWidth; x++)
        {
            for (int y = 0; y < settings.mapHeight; y++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.GetComponent<Renderer>().material = settings.eptyMaterial;
                go.transform.position = new Vector3(x * settings.cellLength, y * settings.cellLength, 1f) + secondMapStart.transform.position;
                secondPlayerCellsVizual[x, y] = go;
            }
        }
        UpdateVizual(firstPlayerCellsVizual, firstMap, true);
        UpdateVizual(secondPlayerCellsVizual, secondMap, false);
        active = true;
    }

    public void DeleteBattleField()
    {
        fired = false;
        foreach (var go in firstPlayerCellsVizual)
        {
            Destroy(go);
        }
        foreach (var go in secondPlayerCellsVizual)
        {
            Destroy(go);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && active && !EventSystem.current.IsPointerOverGameObject())
        {
            int x;
            int y;
            if (gameController.mode == Mode.FirstPlayerTurn)
            {
                if (GetMapPosFromWorldPos(secondMap, secondMapStart.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y))
                {
                    if (!fired && secondMap.Damage(x, y))
                    {
                        fired = true;
                        UpdateVizual(Mode.FirstPlayerTurn);
                    }
                }
            }
            else if (gameController.mode == Mode.SecondPlayerTurn)
            {
                if (GetMapPosFromWorldPos(firstMap, secondMapStart.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y))
                {
                    if (!fired && firstMap.Damage(x, y))
                    {
                        fired = true;
                        UpdateVizual(Mode.SecondPlayerTurn);
                    }
                }
            }
        }
    }

    public void UpdateVizual(Mode mode)
    {
        switch (mode)
        {
            case Mode.FirstPlayerTurn:
                UpdateVizual(firstPlayerCellsVizual, firstMap, true);
                UpdateVizual(secondPlayerCellsVizual, secondMap, false);
                break;
            case Mode.SecondPlayerTurn:
                UpdateVizual(firstPlayerCellsVizual, secondMap, true);
                UpdateVizual(secondPlayerCellsVizual, firstMap, false);
                break;
        }
    }

    //TODO
    /// <param name="activePlayer">vizual owned by player whose turn is now</param>
    private void UpdateVizual(GameObject[,] cellsVizual, Map map, bool activePlayer/**/)
    {
        if (activePlayer)
        {
            for (int x = 0; x < settings.mapWidth; x++)
            {
                for (int y = 0; y < settings.mapHeight; y++)
                {
                    Material materialForCell = settings.eptyMaterial;
                    switch (map.cells[x, y].cellType)
                    {
                        case CellType.Blocked:
                            materialForCell = settings.eptyMaterial;
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
                        case CellType.Miss:
                            materialForCell = settings.missMaterial;
                            break;
                    }
                    cellsVizual[x, y].GetComponent<Renderer>().material = materialForCell;
                }
            }
        }
        else
        {
            for (int x = 0; x < settings.mapWidth; x++)
            {
                for (int y = 0; y < settings.mapHeight; y++)
                {
                    Material materialForCell = settings.eptyMaterial;
                    switch (map.cells[x, y].cellType)
                    {
                        case CellType.Blocked:
                            materialForCell = settings.eptyMaterial;
                            break;
                        case CellType.Empty:
                            materialForCell = settings.eptyMaterial;
                            break;
                        case CellType.Live:
                            materialForCell = settings.eptyMaterial;
                            break;
                        case CellType.Dead:
                            materialForCell = settings.deadMaterial;
                            break;
                        case CellType.Miss:
                            materialForCell = settings.missMaterial;
                            break;
                    }
                    cellsVizual[x, y].GetComponent<Renderer>().material = materialForCell;
                }
            }
        }
    }

    public bool GetMapPosFromWorldPos(Map map, Vector2 mapstart, Vector2 worldPosition, out int x, out int y)
    {
        if ((worldPosition.x >= mapstart.x && worldPosition.x < settings.cellLength * settings.mapWidth + mapstart.x) &&
            (worldPosition.y >= mapstart.y && worldPosition.y < settings.cellLength * settings.mapHeight + mapstart.y))
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
