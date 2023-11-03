using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string buildingName;
    public int uniqueID;
    public int buildingLevel;
    public int cost;
    public int income;

    public Tile position;
    public Tile direction;
}
