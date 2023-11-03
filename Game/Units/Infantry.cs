using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Unit
{
    public override void Init(Tile destination)
    {
        base.Init(destination);
    }

    public override IEnumerator Do()
    {
        return base.Do();
    }

    public override void Attack()
    {
    }

    public override void Die()
    {
    }
}
