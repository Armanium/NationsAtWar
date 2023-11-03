using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private GridManager gridManager;
    private BuildingManager buildingManager;
    private UnitPoolManager pool;

    public bool isConnected = false;

    public Player player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        gridManager = FindObjectOfType<GridManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
        pool = FindObjectOfType<UnitPoolManager>();

        StartCoroutine(waitfor());
    }

    public IEnumerator waitfor()
    {
        yield return new WaitForSeconds(0.1f);
        InitalizeMap();
    }

    private void InitalizeMap()
    {
        gridManager.LoadMap();


        // Debug

        buildingManager.CreateBuilding("Base", gridManager.grid["[9,0,-9]"], gridManager.grid["[8,0,-8]"]);
        buildingManager.buildings[0].playable = true;
        buildingManager.CreateBuilding("Base", gridManager.grid["[-9,0,9]"], gridManager.grid["[-8,0,8]"]);

        buildingManager.CreateBuilding("Oil", gridManager.grid["[5,-2,-3]"], gridManager.grid["[4,-2,-2]"]);
        buildingManager.CreateBuilding("Oil", gridManager.grid["[-3,-2,5]"], gridManager.grid["[-4,-2,6]"]);
        buildingManager.CreateBuilding("Oil", gridManager.grid["[2,-4,2]"], gridManager.grid["[1,-4,3]"]);
        buildingManager.CreateBuilding("Oil", gridManager.grid["[-5,2,3]"], gridManager.grid["[-6,2,4]"]);
        buildingManager.CreateBuilding("Oil", gridManager.grid["[3,2,-5]"], gridManager.grid["[2,2,-4]"]);
        buildingManager.CreateBuilding("Oil", gridManager.grid["[-2,4,-2]"], gridManager.grid["[-3,4,-1]"]);

        Unit unit = pool.Dequeue("Infantry");

        buildingManager.buildings[0].CreateUnit(unit);

        //
    }
}
