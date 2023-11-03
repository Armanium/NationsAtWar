using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    private GridManager gridManager;
    private Pathfinding pathfinding;

    public Camera cam;

    public Tile selectedTile;
    public Unit selectedUnit;
    public Building selectedBuilding;

    public List<Tile> lastWaypoints = new List<Tile>();


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        gridManager = FindAnyObjectByType<GridManager>();
        pathfinding = FindAnyObjectByType<Pathfinding>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed");
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = gridManager.PosToHex(hit.point);

                HandleGridSelection(tile);
            }
        }
        
    }

    private void HandleGridSelection(Tile tile)
    {
        if(tile == null)
        {
            RevertSelection();

            return;
        }

        if(selectedTile == null)
        {
            ShowRange(tile);
        }
        else
        {
            switch (tile.action)
            {
                case "Null":

                    RevertSelection();
                    
                    return;

                case "Move":

                    tile.action = "Destination";
                    tile.ChangeState();
                    ShowWaypoints(tile);

                    break;

                case "Destination":

                    ApplyMovement();

                    break;
            }
            
        }
    }

    private void RevertSelection()
    {
        selectedTile = null;
        selectedBuilding = null;
        selectedUnit = null;

        foreach(Tile tile in gridManager.grid.Values)
        {
            tile.RevertChanges();
        }
    }

    private void ShowRange(Tile tile)
    {
        if (tile.unit != null)
        {
            selectedTile = tile;
            selectedUnit = selectedTile.unit;

            List<Tile> moveable = pathfinding.GetRange(tile, tile.unit.data.range, 100);

            selectedTile.unit.data.movementRange = moveable;

            gridManager.HightlightMoveable(moveable);
        }
        if (tile.building != null)
        {

        }
    }

    private void ShowWaypoints(Tile tile)
    {
        List<Tile> waypoints = pathfinding.GetPath(selectedTile.unit.data.movementRange, selectedTile, tile);
        if (lastWaypoints.Count != 0)
        {
            for (int i = 0; i < lastWaypoints.Count; i++)
            {
                lastWaypoints[i].action = "Move";
                lastWaypoints[i].ChangeState();
            }
        }

        selectedUnit.data.waypoints = waypoints;

        lastWaypoints = waypoints;
        gridManager.HightlightWaypoints(waypoints);
    }

    private void ApplyMovement()
    {
        selectedUnit.data.waypoints = lastWaypoints;

        gridManager.ClearAllTileAction();

        StartCoroutine(selectedUnit.Do());
    }

    private void ShowAttackPosition()
    {

    }

    private void ApplyAttack()
    {

    }

    private void ShowBuildingUI()
    {

    }

    private void BuildingCreateUnit()
    {

    }

    private void BuildingDestroy()
    {

    }

    private void BuildingCreate()
    {

    }

    // Percs
}
