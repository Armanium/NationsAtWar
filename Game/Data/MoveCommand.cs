using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    public string UnitID;
    public List<string> waypoints;
}
