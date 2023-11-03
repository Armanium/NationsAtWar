using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Building : MonoBehaviour
{
    public BuildingData data;

    public bool playable;

    public virtual void Create(Tile position, Tile direction)
    {
        data.position = position;
        data.direction = direction;
        position.moveable = false;
    }

    public virtual void CreateUnit(Unit unit)
    {
        unit.transform.position = data.position.transform.position;
        unit.gameObject.SetActive(true);

        unit.Init(data.direction);
    }

    public virtual void Action()
    {

    }

    public virtual void Destroy()
    {

    }

    public virtual void Income()
    {

    }
}
