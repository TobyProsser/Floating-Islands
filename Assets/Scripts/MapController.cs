using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public int teams;

    public int size;
    public int distanceToMainIsland;

    public GameObject mainIsland;
    public GameObject spawnIsland;
    public GameObject lootIsland;

    void Start()
    {
        FindIslandLocations();
        SpawnMainIsland();

        BuildMesh();
    }

    void FindIslandLocations()
    {
        float angle = 360 / teams;

        List<Vector2> positions = new List<Vector2>();

        float curAngle = 0;
        for (int i = 0; i < teams; i++)
        {
            float radian = curAngle * Mathf.Deg2Rad;

            Vector2 newPos = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            newPos *= (size/2 + distanceToMainIsland);

            positions.Add(newPos);

            curAngle += angle;
        }

        SpawnIslands(positions);
        LootIslands(positions);
    }

    void SpawnMainIsland()
    {
        GameObject curMainIsland = Instantiate(mainIsland, Vector3.zero, Quaternion.identity);
        SpawnIslandGenerator islandGenerator = curMainIsland.GetComponent<SpawnIslandGenerator>();
        islandGenerator.size = size;
        islandGenerator.amp = size * 0.5f + 10;

        islandGenerator.GenerateIsland();
    }

    void SpawnIslands(List<Vector2> positions)
    {
        foreach (Vector2 pos in positions)
        {
            GameObject curSpawnIsland = Instantiate(spawnIsland, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
            SpawnIslandGenerator islandGenerator = curSpawnIsland.GetComponent<SpawnIslandGenerator>();

            islandGenerator.GenerateIsland();
        }
    }

    void LootIslands(List<Vector2> positions)
    {
        foreach (Vector2 pos in positions)
        {
            Vector2 adjPos = pos * 1.5f;
            GameObject curLootIsland = Instantiate(lootIsland, new Vector3(adjPos.x, 0,adjPos.y), Quaternion.identity);
            SpawnIslandGenerator islandGenerator = curLootIsland.GetComponent<SpawnIslandGenerator>();

            islandGenerator.GenerateIsland();
        }
    }


    void BuildMesh()
    {
        NavMeshController.navSurface.RemoveData();
        NavMeshController.navSurface.BuildNavMesh();
        print("navmesh built");
    }
}
