using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitData data;

    public virtual void Init(Tile destination)
    {
        data.waypoints.Add(destination);

        data.position = destination;
        destination.unit = this;

        StartCoroutine(Do());
    }


    public virtual IEnumerator Do()
    {
        while(data.waypoints.Count > 0)
        {
            transform.LookAt(data.waypoints[0].transform.position);  //(Utils.CalculateDirection(transform.position, data.waypoints[0].transform.position));

            if(transform.position == data.waypoints[0].transform.position)
            {
                data.position.unit = null;

                data.position = data.waypoints[0];
                data.position.unit = this;

                data.waypoints.RemoveAt(0);

                yield return new WaitForSeconds(0.3f);

                continue;
            }
            transform.position = Vector3.MoveTowards(transform.position, data.waypoints[0].transform.position, data.speed * Time.deltaTime);


            yield return new WaitForFixedUpdate();
        }
    }

    public virtual void Attack()
    {

    }

    public virtual void Die()
    {

    }

}
