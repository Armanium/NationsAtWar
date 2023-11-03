using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int uniqueID;
    public string name;
    public string description;
    public int price;
    public float speed;
    public float damage;
    public float armor;
    public float health;
    public int quantity;
    public int range;
    public int attackRange;

    public bool playable;

    public Tile position;

    public List<UnitMoveCost> moveCost;

    public List<Tile> movementRange;
    public List<Tile> atackable;
    public List<Tile> fov;
    public List<Tile> waypoints;

}
