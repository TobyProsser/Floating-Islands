using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    public GameObject cube;
    public GameObject tree;
    public GameObject shortGrass;
    public GameObject tallGrass;

    public Material material;
    public List<Color> grassColors = new List<Color>();
    public List<Color> stoneColors = new List<Color>();

    public int size;
    public float amp;
    public float freq;

    public float groundAmp;

    public Vector2 offset;

    public int groundLevels = 0;

    [HideInInspector]
    public List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        GenerateBase();
    }

    void GenerateBase()
    {
        Vector3 pos = this.transform.position - new Vector3(size / 2, 0, size / 2);
        int cols = size;
        int rows = size;

        for (int x = 0; x < cols; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                float y = Mathf.PerlinNoise(x / freq + offset.x, z / freq + offset.y) * amp;

                float radius = size / 2;

                if (Vector2.Distance(new Vector2(cols/2, rows/2), new Vector2(x, z)) < radius)
                {
                    if (Mathf.Abs(y) > 20)
                    {
                        GameObject curCube = GameObject.Instantiate(cube);
                        curCube.transform.localScale = new Vector3(1, 1 + (y*2) - 40, 1);
                        curCube.transform.position = new Vector3(pos.x + x, -y, pos.z + z);
                        curCube.GetComponent<Renderer>().material.color = stoneColors[Random.Range(0, stoneColors.Count)];
                        curCube.tag = "Stone";

                        if (Mathf.Abs(y) > 23)
                        {
                            float groundYScale = FindGroundYScale(y);

                            bool stone = false;
                            if (groundYScale >= 4) stone = true;

                            float groundY = curCube.transform.position.y + (curCube.transform.localScale.y/2) + (groundYScale / 2);
                            
                            GameObject topCube = GameObject.Instantiate(cube);
                            topCube.transform.position = new Vector3(pos.x + x, groundY, pos.z + z);
                            topCube.transform.localScale = new Vector3(1, groundYScale, 1);
                            topCube.transform.parent = this.transform;

                            if (!stone)
                            {
                                topCube.GetComponent<Renderer>().material.color = grassColors[Random.Range(0, grassColors.Count)];
                                topCube.tag = "Grass";
                                spawnPoints.Add(topCube.transform);

                                int grassChance = Random.Range(0, 5);
                                if (grassChance == 0)
                                {
                                    int tallChance = Random.Range(0, 10);

                                    GameObject curGrass = ((tallChance == 0) ? Instantiate(tallGrass) : Instantiate(shortGrass));
                                    curGrass.transform.position = new Vector3(pos.x + x, topCube.transform.position.y + topCube.transform.localScale.y / 2, pos.z + z);
                                    curGrass.transform.Rotate(new Vector3(0, 90 * Random.Range(0, 5), 0));
                                    curGrass.transform.parent = transform;
                                }

                                int treeChance = Random.Range(0, 50);
                                if (treeChance == 0)
                                {
                                    GameObject curTree = GameObject.Instantiate(tree);
                                    curTree.transform.position = new Vector3(pos.x + x, topCube.transform.position.y + topCube.transform.localScale.y / 2, pos.z + z);
                                    curTree.transform.parent = this.transform;
                                }
                            }
                            else
                            {
                                topCube.GetComponent<Renderer>().material.color = stoneColors[Random.Range(0, stoneColors.Count)];
                                topCube.tag = "Stone";

                                topCube = GameObject.Instantiate(cube);
                                topCube.transform.position = new Vector3(pos.x + x, groundY + (groundYScale / 2) + 0.5f, pos.z + z);
                                topCube.transform.localScale = new Vector3(1, 1, 1);
                                topCube.transform.parent = this.transform;
                                topCube.GetComponent<Renderer>().material.color = grassColors[Random.Range(0, grassColors.Count)];
                                topCube.tag = "Grass";

                                if (groundYScale <= 3 || groundYScale == 10) spawnPoints.Add(topCube.transform);
                            }
                        }
                        else
                        {
                            float yIncrease = Random.Range(.5f, 4);
                            curCube.transform.localScale += new Vector3(0, yIncrease, 0);
                            curCube.transform.position += new Vector3(0, yIncrease/2, 0);
                        }

                        curCube.transform.parent = this.transform;
                    }
                }
            }
        }

        IslandObjectSpawner islandObjectSpawner = this.GetComponent<IslandObjectSpawner>();
        islandObjectSpawner.IslandSpawned(spawnPoints);

        NavMeshController.navSurface.RemoveData();
        NavMeshController.navSurface.BuildNavMesh();
    }

    float FindGroundYScale(float y)
    {
        float groundYScale = (y / amp);

        if (groundLevels == 0)
        {
            groundYScale = 1;
        }
        else if (groundLevels == 1)
        {
            if (groundYScale < .6f) groundYScale = 1;
            else if (groundYScale < .7f) groundYScale = 2;
        }
        else if (groundLevels == 2)
        {
            if (groundYScale < .6f) groundYScale = 1;
            else if (groundYScale < .7f) groundYScale = 2;
            else groundYScale = 3;
        }
        else if (groundLevels == 3)
        {
            if (groundYScale < .6f) groundYScale = 1;
            else if (groundYScale < .7f) groundYScale = 2;
            else if (groundYScale < .83f) groundYScale = 3;
            else if (groundYScale < .85f) groundYScale = 5;
            else if (groundYScale < .88f) groundYScale = 6;
            else if (groundYScale < .9f) groundYScale = 7;
            else if (groundYScale < .93f) groundYScale = 9;
            else groundYScale = 10;
        }

        return groundYScale;
    }
}
