using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 1)]
public class Settings : ScriptableObject
{
    public int mapWidth = 10;
    public int mapHeight = 10;
    public float cellLength = 1f;
    public List<ShipSettings> ships;

    public Material blockedMaterial;
    public Material deadMaterial;
    public Material shipMaterial;
    public Material eptyMaterial;
    public Material missMaterial;
}


