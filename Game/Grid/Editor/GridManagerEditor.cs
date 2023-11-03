using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridManager manager = (GridManager)target;

        if (GUILayout.Button("Initalize map"))
        {
            manager.InitalizeMap();
        }
        if (GUILayout.Button("Load map"))
        {
            manager.LoadMap();
        }
        if (GUILayout.Button("Save map"))
        {
            manager.SaveMap();
        }

        if (GUILayout.Button("Show all"))
        {
            foreach (Tile tile in manager.grid.Values)
            {
                tile.action = "Move";
                tile.ChangeState();
            }
        }

        if (GUILayout.Button("Highlight grass"))
        {
            List<Tile> grass = new List<Tile>();

            foreach(Tile tile in manager.grid.Values)
            {
                if(tile.types == Tile.tileType.grass)
                {
                    grass.Add(tile);
                }
            }

            manager.HighlightTilesWithState(grass);
        }

        if (GUILayout.Button("Highlight road"))
        {
            List<Tile> road = new List<Tile>();

            foreach (Tile tile in manager.grid.Values)
            {
                if (tile.types == Tile.tileType.road)
                {
                    road.Add(tile);
                }
            }

            manager.HighlightTilesWithState(road);
        }

        if (GUILayout.Button("Highlight forest"))
        {
            List<Tile> forest = new List<Tile>();

            foreach (Tile tile in manager.grid.Values)
            {
                if (tile.types == Tile.tileType.forest)
                {
                    forest.Add(tile);
                }
            }

            manager.HighlightTilesWithState(forest);
        }

        if (GUILayout.Button("Hightlight rock"))
        {
            List<Tile> rock = new List<Tile>();

            foreach (Tile tile in manager.grid.Values)
            {
                if (tile.types == Tile.tileType.rock)
                {
                    rock.Add(tile);
                }
            }

            manager.HighlightTilesWithState(rock);
        }

        if (GUILayout.Button("Highlight dirt"))
        {
            List<Tile> dirt = new List<Tile>();

            foreach (Tile tile in manager.grid.Values)
            {
                if (tile.types == Tile.tileType.dirt)
                {
                    dirt.Add(tile);
                }
            }

            manager.HighlightTilesWithState(dirt);
        }

    }
}
