using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPoolManager : MonoBehaviour
{
    public static UnitPoolManager instance = null;

    public Dictionary<string,Queue<Unit>> pools = new Dictionary<string, Queue<Unit>>();

    public Dictionary<string, int> toInit = new Dictionary<string, int>()
    {
        { "Infantry" , 10 }
    };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Initalize();
    }

    public void Initalize()
    {
        foreach (KeyValuePair<string,int> unit in toInit) 
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Units/" + unit.Key);

            for (int i = 0; i < unit.Value; i++)
            {
                var inst = Instantiate(prefab).GetComponent<Unit>();

                string n = inst.GetType().Name;

                if(!pools.ContainsKey(n))
                {
                    pools.Add(n, new Queue<Unit>());
                    Debug.Log("Pools added: " + "|" + n + "|");
                    inst.gameObject.SetActive(false);

                    continue;
                }

                pools[n].Enqueue(inst);

                inst.transform.position = new Vector3(0, 100, 0);
                inst.gameObject.SetActive(false);
            }
        }      
    }

    public Unit Dequeue(string unitType)
    {
        Debug.Log("|"+unitType+"|");
        Unit first = pools[unitType].Dequeue();

        first.gameObject.SetActive(true);

        return first;
    }

    public void Enqueue(Unit unit)
    {
        string type = nameof(unit);
        pools[type].Enqueue(unit);

        unit.gameObject.SetActive(false);
    }
}
