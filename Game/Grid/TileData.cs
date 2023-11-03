using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public TileData(TileIndex tileIndex, Tile.tileType tileType)
    {
        this.index = tileIndex;
        this.tileType = tileType;
    }
    public TileIndex index;
    public Tile.tileType tileType;
}
