using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuildingsController : MonoBehaviour
{
    public List<GameObject> villageBuildings = new List<GameObject>();

    List<GameObject> emptyBuildings = new List<GameObject>();

    //ran by islandObjectSpawner
    public void OrganizeBuildings(List<GameObject> buildings)
    {
        print(buildings.Count);
        emptyBuildings = buildings;
        if (emptyBuildings.Count < 10)
        {
            print("Not enough Buildings");
            return;
        }

        //replace all empty buildings with village buildings
        for (int i = 0; i < emptyBuildings.Count; i++)
        {
            if (i < 10)
            {
                ReplaceBuilding(i, i);
            }
        }
        print("Replaced");
        //if there are any extra buildings left over,
        //replace them with homes and farms
        if (emptyBuildings.Count > 0)
        {
            for (int i = 0; i < emptyBuildings.Count; i++)
            {
                if (i % 2 == 0) ReplaceBuilding(i, 0);
                else ReplaceBuilding(i, 1);
            }
        }
    }

    void ReplaceBuilding(int one, int two)
    {
        GameObject curBuilding = Instantiate(villageBuildings[two], emptyBuildings[one].transform.position, emptyBuildings[one].transform.rotation);
        Destroy(emptyBuildings[one]);
        emptyBuildings.Remove(emptyBuildings[one]);
    }
}
