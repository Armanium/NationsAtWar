using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;
using System;

public class GridManager : MonoBehaviour
{
    static GridManager instance;

    public int size;
    public float scale;

    [SerializeField]
    private GameObject hexPrefab;

    // Parent of tiles
    public Transform tiles;

    public Dictionary<string, Tile> grid = new Dictionary<string, Tile>(); // List of all Tile

    // Neighbour directions
    private TileIndex[] directions =
        new TileIndex[] {
            new TileIndex(1, -1, 0),
            new TileIndex(1, 0, -1),
            new TileIndex(0, 1, -1),
            new TileIndex(-1, 1, 0),
            new TileIndex(-1, 0, 1),
            new TileIndex(0, -1, 1)
        };


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        //InitalizeMap();
    }

    public void InitalizeMap()
    {
        CleanMap();
        grid.Clear();
        Tile tile;
        Vector3 pos = Vector3.zero;

        int mapSize = size;

        for (int x = -mapSize; x <= mapSize; x++)  // Create grid loop
        {
            int r1 = Mathf.Max(-mapSize, -x - mapSize);
            int r2 = Mathf.Min(mapSize, -x + mapSize);

            for (int y = r1; y <= r2; y++)
            {
                pos.x = size * Mathf.Sqrt(3.0f) * (x + y / 2.0f);
                pos.z = size * 3.0f / 2.0f * y;

                tile = Instantiate(hexPrefab).GetComponent<Tile>();

                tile.transform.parent = tiles;
                tile.transform.position = pos;

                tile.transform.localScale = Vector3.one * scale;

                tile.name = "tile (" + x + "," + y + "," + (-x - y) + ")";

                tile.index = new TileIndex(x, y, -x - y);

                grid.Add(tile.index.ToString(), tile);
            }
        }
    }

    public void LoadMap()
    {
        CleanMap();
        grid.Clear();

        Vector3 pos = Vector3.zero;
        Tile tile;

        string serialized = System.IO.File.ReadAllText(Application.dataPath + "/map.json");

        List<TileData> tileData = JsonConvert.DeserializeObject<List<TileData>>(serialized);

        for (int i = 0; i < tileData.Count; i++)
        {

            pos.x = size * Mathf.Sqrt(3.0f) * (tileData[i].index.x + tileData[i].index.y / 2.0f);
            pos.z = size * 3.0f / 2.0f * tileData[i].index.y;

            tile = Instantiate(hexPrefab).GetComponent<Tile>();

            tile.types = tileData[i].tileType;

            tile.action = "Move";
            tile.ChangeState();

            tile.transform.parent = tiles;
            tile.transform.position = pos;

            tile.transform.localScale = Vector3.one * scale;

            tile.name = "tile (" + tileData[i].index.x + "," + tileData[i].index.y + "," + (-tileData[i].index.x - tileData[i].index.y) + ")";

            tile.index = new TileIndex(tileData[i].index.x, tileData[i].index.y, -tileData[i].index.x - tileData[i].index.y);

            grid.Add(tile.index.ToString(), tile);
        }

        Debug.Log("Grid contains: " + grid.Count);
    }

    public void SaveMap()
    {
        List<TileData> tiles = new List<TileData>();

        foreach (Transform t in this.tiles)
        {
            Tile tile = t.GetComponent<Tile>();

            tiles.Add(new TileData(tile.index, tile.types));
        }

        string serialized = JsonConvert.SerializeObject(tiles);

        System.IO.File.WriteAllText(Application.dataPath + "/map.json", serialized);
    }

    public void CleanMap()
    {
        grid.Clear();
        foreach (Transform t in tiles)
        {
            Destroy(t.gameObject);
        }
    }

    public Vector3 HexToPos(Tile tile)
    {
        var x = size * (Mathf.Sqrt(3f) * tile.index.x + Mathf.Sqrt(3f) / 2f * tile.index.y);
        var y = size * (3f / 2f * tile.index.y);

        return new Vector3(x, 0, y);
    }

    public Tile PosToHex(Vector3 pos)
    {
        var x = (Mathf.Sqrt(3f) / 3f * pos.x - 1f / 3f * pos.z) / size;
        var y = (2f / 3f * pos.z) / size;



        TileIndex index = RoundToTile(new Vector3(x, y, -x - y));

        if (grid.ContainsKey(index.ToString()))
        {
            return grid[index.ToString()];
        }
        else
        {
            return null;
        }
    }

    private TileIndex RoundToTile(Vector3 fraq)
    {

        float x = Mathf.Round(fraq.x);
        float y = Mathf.Round(fraq.y);
        float z = Mathf.Round(fraq.z);

        float q_diff = Mathf.Abs(x - fraq.x);
        float r_diff = Mathf.Abs(y - fraq.y);
        float s_diff = Mathf.Abs(z - fraq.z);

        if (q_diff > r_diff && q_diff > s_diff)
        {
            x = -y - z;
        }
        else
        {
            if (r_diff > s_diff)
            {
                y = -x - z;
            }
            else
            {
                z = -x - y;
            }
        }
        return new TileIndex((int)x, (int)y, (int)z);
    }

    public List<Tile> Neighbours(Tile tile)
    {
        List<Tile> ret = new List<Tile>();

        if (tile == null)
            return ret;

        TileIndex o;

        for (int i = 0; i < 6; i++)
        {
            o = tile.index + directions[i];
            if (grid.ContainsKey(o.ToString()))
            {
                if (grid[o.ToString()] != null)
                    ret.Add(grid[o.ToString()]);
            }
            /*
            else
            {
                ret.Add(null);
            }
            */
        }
        return ret;
    }

    public List<Tile> TilesInRange(Tile center, int range)
    {

        List<Tile> ret = new List<Tile>();
        TileIndex o;

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                o = new TileIndex(dx, dy, -dx - dy) + center.index;
                if (grid.ContainsKey(o.ToString()))
                    ret.Add(grid[o.ToString()]);
            }
        }
        return ret;
    }

    public float ManhattanDistance(Tile a, Tile b)
    {
        return (Mathf.Abs(a.index.x - b.index.x) + Mathf.Abs(a.index.y - b.index.y) + Mathf.Abs(a.index.z - b.index.z)) / 2f;
    }

    public void HightlightMoveable(List<Tile> moveable)
    {
        for (int i = 0; i < moveable.Count; i++)
        {
            moveable[i].gameObject.SetActive(true);
            moveable[i].action = "Move";
            moveable[i].ChangeState();
        }
    }

    public void HightlightAttackable(List<Tile> attackable)
    {
        for (int i = 0; i < attackable.Count; i++)
        {
            attackable[i].gameObject.SetActive(true);
            attackable[i].action = "Move";
            attackable[i].ChangeState();
        }
    }

    public void HightlightAttackPositions(List<Tile> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i].gameObject.SetActive(true);
            positions[i].action = "Move";
            positions[i].ChangeState();
        }
    }

    public void HightlightWaypoints(List<Tile> waypoints)
    {
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            waypoints[i].nextWaypoint = waypoints[i + 1];
            waypoints[i].action = "Waypoint";
            waypoints[i].ChangeState();
        }
        
    }

    public void ClearAllTileAction()
    {
        foreach (Tile tile in grid.Values)
        {
            tile.action = "Null";
            tile.ChangeState();
        }
    }

    public void HighlightTilesWithState(List<Tile> tiles)
    {
        ClearAllTileAction();

        Debug.Log("tiles count: " + tiles.Count);
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].action = "Move";
            tiles[i].ChangeState();
        }
    }
}
