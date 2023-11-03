using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public List<Building> buildings = new List<Building>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void CreateBuilding(string buildingType,Tile position ,Tile direction)
    {
        Building building = GameObject.Instantiate(Resources.Load("Prefabs/Buildings/" + buildingType ) as GameObject, position.transform.position, Quaternion.identity).GetComponent<Building>();

        building.transform.forward = Utils.CalculateDirection(position.transform.position, direction.transform.position);

        building.Create(position, direction);

        buildings.Add(building.GetComponent<Building>());
    }

    public void DestroyBuilding(Building building)
    {
        building.Destroy();

        buildings.Remove(building);
    }
}
