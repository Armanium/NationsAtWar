using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : Building
{
    public enum control
    {
        Red,
        Blue,
        Neutral
    }

    public control state;

    public override void Create(Tile position, Tile direction)
    {
        data.position = position;
        data.direction = direction;

        transform.LookAt(direction.transform.position);

        state = control.Neutral;
    }
}
