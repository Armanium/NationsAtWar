using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[SelectionBase]
public class Tile : MonoBehaviour
{
    public enum tileType
    {
        grass,
        dirt,
        rock,
        road,
        forest
    }

    public tileType types;

    private MeshRenderer rend;
    private Transform pointer;

    public TileIndex index;

    public Building building;
    public Unit unit;

    public string action;

    public Tile nextWaypoint;
    public Unit enemyUnit;
    public Building enemyBuilding;

    public Material defaultMat;
    public Material destionationMat;
    public Material moveMat;
    public Material enemyMat;

    [Header("Pathfinding")]
    public Tile parent;
    public float cost;
    public float f;
    public float d;
    public float t;
    public bool moveable = true;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        pointer = transform.GetChild(0);
    }

    public void ChangeState()
    {
        switch(action)
        {
            case "Null":

                rend.material = defaultMat;

                break;

            case "Destination":

                rend.material = destionationMat;

                break;

            case "Waypoint":

                rend.material = moveMat;
                SetWaypointPointerRotation();

                break;

            case "Move":

                rend.material = moveMat;
                nextWaypoint = null;
                RemoveWaypointPointer();

                break;

            case "Enemy":

                rend.material = enemyMat;

                break;

            case "AttackPosition":

                rend.material = enemyMat;

                break;
        }
    }

    public void SetWaypointPointerRotation()
    {
        pointer.gameObject.SetActive(true);

        pointer.transform.forward = Utils.CalculateDirection(pointer.position, nextWaypoint.transform.position);
    }

    public void RemoveWaypointPointer()
    {
        pointer.gameObject.SetActive(false);
    }

    public void RevertChanges()
    {
        action = "Null";
        nextWaypoint = null;
        f = 0;
        d = 0;
        t = 0;
        RemoveWaypointPointer();

        gameObject.SetActive(false);
    }
}
