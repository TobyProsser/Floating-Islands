using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandObjectSpawner : MonoBehaviour
{
    public int chestsAmount = 1;
    public GameObject chest;

    public bool spawnBuildings;
    public int buildingsAmount = 1;
    public GameObject building;

    public int spawnersAmount = 1;
    public GameObject spawner;

    int buildingTryAmount = 0;

    List<Transform> spawnLocations = new List<Transform>();

    VillageBuildingsController villageBuildingsController;
    public List<GameObject> spawnedEmptyBuildings = new List<GameObject>();
    public List<GameObject> spawnBuildingList = new List<GameObject>();
    bool buildingsSpawned;

    private void Awake()
    {
        villageBuildingsController = this.GetComponent<VillageBuildingsController>();
    }
    public void IslandSpawned(List<Transform> spawnLocs)
    {
        spawnLocations = spawnLocs;
        SpawnChests();
        SpawnSpawners();

        for (int i = 0; i < buildingsAmount; i++)
        {
            SpawnBuilding();
        }

        NavMeshController.navSurface.BuildNavMesh();
    }

    private void LateUpdate()
    {
        if (spawnedEmptyBuildings.Count == buildingsAmount && !buildingsSpawned)
        {
            buildingsSpawned = true;
            if (spawnBuildings)
            {
                ReplaceBuildings();
            }
            else villageBuildingsController.OrganizeBuildings(spawnedEmptyBuildings);
        }
    }
    void SpawnChests()
    {
        for (int i = 0; i < chestsAmount; i++)
        {
            GameObject curChest = Instantiate(chest);
            curChest.transform.parent = this.transform;
            Transform spawnCubeTrans = FindChestSpawnLoc();
            Vector3 spawnPos = spawnCubeTrans.localPosition + new Vector3(.5f, .5f + (spawnCubeTrans.lossyScale.y / 2), 0);
            curChest.transform.localPosition = spawnPos;
        }
    }

    Transform FindChestSpawnLoc()
    {
        int start = Random.Range(0, spawnLocations.Count -1);
        Transform trans = null;

        for (int i = start; i < spawnLocations.Count -1; i++)
        {
            if (spawnLocations[i].position.y == spawnLocations[i + 1].position.y)
            {
                trans = spawnLocations[i];
                break;
            }

            print(spawnLocations[i].position.y + " " + spawnLocations[i + 1].position.y);
        }

        if (trans == null)
        {
            for (int i = 0; i < spawnLocations.Count -1; i++)
            {
                if (spawnLocations[i].position.y == spawnLocations[i + 1].position.y)
                {
                    trans = spawnLocations[i];
                    break;
                }
            }
        }

        return trans;
    }

    public void SpawnBuilding()
    {
        if (buildingTryAmount < 6000)
        {
            int rand = Random.Range(0, spawnLocations.Count);

            Transform spawnCube = spawnLocations[rand];

            Vector3 spawnLoc = spawnCube.position + new Vector3(0, + (spawnCube.lossyScale.y / 2) - .5f,0);
            GameObject curBuilding = Instantiate(building, spawnLoc, Quaternion.identity);
            curBuilding.GetComponent<BuildingClearScript>().islandObjectSpawner = this;

            buildingTryAmount++;
        }
    }

    void SpawnSpawners()
    {
        for (int i = 0; i < spawnersAmount; i++)
        {
            GameObject curSpawner = Instantiate(spawner);
            curSpawner.transform.parent = this.transform;
            Transform spawnCubeTrans = spawnLocations[Random.Range(0, spawnLocations.Count)];
            Vector3 spawnPos = spawnCubeTrans.localPosition + new Vector3(.5f, 1.5f + (spawnCubeTrans.lossyScale.y / 2), 0);
            curSpawner.transform.localPosition = spawnPos;

            curSpawner.GetComponent<EnemySpawnerController>().spawnPoints = spawnLocations;
        }
    }

    void ReplaceBuildings()
    {
        print(spawnedEmptyBuildings.Count);
        GameObject curBuilding = Instantiate(spawnBuildingList[0], spawnedEmptyBuildings[0].transform.position, spawnedEmptyBuildings[0].transform.rotation);
        Destroy(spawnedEmptyBuildings[0]);
        spawnedEmptyBuildings.Remove(spawnedEmptyBuildings[0]);

        GameObject curBuilding1 = Instantiate(spawnBuildingList[1], spawnedEmptyBuildings[0].transform.position, spawnedEmptyBuildings[0].transform.rotation);
        Destroy(spawnedEmptyBuildings[0]);
        spawnedEmptyBuildings.Remove(spawnedEmptyBuildings[0]);
    }
}
