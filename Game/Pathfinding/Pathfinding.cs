using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    private GridManager grid;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        grid = FindAnyObjectByType<GridManager>();
    }

    public List<Tile> GetRange(Tile start, int range, int cost)
    {
        List<Tile> closed = new List<Tile>();
        List<Tile> open = new List<Tile>();

        start.f = cost;

        open.Add(start);

        Tile current;

        int iter = 0;

        while(open.Count > 0)
        {
            iter++;
            if(iter == 200)
            {
                Debug.LogError("!!! GetRange Too many iterations !!!");

                break;
            }

            current = open[0];

            List<Tile> nbr = grid.Neighbours(current);

            for (int i = 0; i < nbr.Count; i++)
            {
                if (closed.Contains(nbr[i]) || open.Contains(nbr[i]))
                {
                    continue;
                }

                nbr[i].parent = current;
                nbr[i].d = grid.ManhattanDistance(start, nbr[i]);

                if (nbr[i].d <= range && nbr[i].moveable)
                {
                    nbr[i].f = nbr[i].parent.f - nbr[i].cost;
                    open.Add(nbr[i]);
                }
            }

            open.Remove(current);
            closed.Add(current);
        }

        closed.RemoveAll(x => x.f < 0);
        closed.Remove(start);

        return closed;
    }

    public List<Tile> GetPath(List<Tile> range, Tile start, Tile end)
    {
        List<Tile> closed = new List<Tile>();

        Tile current = start;

        closed.Add(current);

        TilesCalculateD(range, end);
        TilesCalculateT(range);
        

        int iter = 0;

        while (current != end)
        {
            iter++;
            if(iter == 200)
            {
                Debug.LogError("!!! GetPath too mant iterations !!!");
                return null;
            }

            List<Tile> nbr = grid.Neighbours(current);
            nbr.RemoveAll(x => !range.Contains(x));

            if(nbr.Contains(end))
            {
                closed.Add(end);

                break;
            }

            nbr.Sort((x1, x2) => x1.t.CompareTo(x2.t));

            current = nbr[0];
            closed.Add(current);
        }

        closed.Remove(start);

        return closed;
    }

    Dictionary<Unit, List<Tile>> enemyPath = new Dictionary<Unit, List<Tile>>();
    public List<Tile> GetFOV(List<Tile> tiles, int range)
    {
        List<Tile> fov = new List<Tile>();
        List<EnemyPathData> enemy = new List<EnemyPathData>();

        for (int i = 0; i < tiles.Count; i++)
        {
            List<Tile> rng = grid.TilesInRange(tiles[i], range);

            TilesCalculateD(rng, tiles[i]);

            List<Tile> border = GetBorderTiles(rng);

            for (int x = 0; x < border.Count; x++)
            {
                fov.AddRange(GetTilesByStep(tiles[i], border[i], range));
            }
        }

        return fov.Distinct().ToList();
    }

    private void TilesCalculateT(List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].t = tiles[i].f * tiles[i].d;
        }
    }

    private void TilesCalculateD(List<Tile> tiles, Tile end)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].d = grid.ManhattanDistance(tiles[i], end);
        }
    }

    private List<Tile> GetBorderTiles(List<Tile> range)
    {
        List<Tile> result = new List<Tile>();

        for (int i = 0; i < range.Count; i++)
        {
            List<Tile> nbr = grid.Neighbours(range[i]);

            if(nbr.Count < 6 || !range.Contains(nbr[i]))
            {
                result.Add(range[i]);
            }
        }

        return result;
    }

    private List<Tile> GetTilesByStep(Tile start, Tile end, int range)
    {
        List<Tile> result = new List<Tile>();

        Vector3 direction = (end.transform.position - start.transform.position).normalized;
        float step = 1 / range;

        for (float i = step; i <= 1; i+=step)
        {
            Tile f = grid.PosToHex(start.transform.position + (direction * i));

            if(f.types != Tile.tileType.rock || !f.moveable)
            {
                if(!f.unit.data.playable)
                {
                    if(!enemyPath.ContainsKey(f.unit))
                    {
                        enemyPath.Add(f.unit, new List<Tile> { start });
                    }

                    enemyPath[f.unit].Add(start);
                }
                result.Add(f);
            }
            else
            {
                break;
            }
        }

        return result;
    }
}
