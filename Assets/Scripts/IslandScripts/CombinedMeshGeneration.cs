using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedMeshGeneration : MonoBehaviour
{
    public GameObject cube;
    public GameObject tree;

    public Material material;

    public int size;
    public float amp;
    public float freq;

    public float groundAmp;

    public Vector2 offset;

    public int groundLevels = 0;

    [HideInInspector]
    public List<Transform> spawnPoints = new List<Transform>();

    List<GameObject> meshObjectList = new List<GameObject>();

    void Start()
    {
        GenerateBase();
    }

    void GenerateBase()
    {
        Vector3 pos = this.transform.position;
        int cols = size;
        int rows = size;

        List<CombineInstance> combine = new List<CombineInstance>();

        MeshFilter blockMesh = Instantiate(cube, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();

        for (int x = 0; x < cols; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                float y = Mathf.PerlinNoise(x / freq + offset.x, z / freq + offset.y) * amp;

                float radius = size / 2;

                if (Vector2.Distance(new Vector2(cols / 2, rows / 2), new Vector2(x, z)) < radius)
                {
                    if (Mathf.Abs(y) > 20)
                    {
                        blockMesh.transform.localScale = new Vector3(1, 1 + (y * 2) - 40, 1);
                        blockMesh.transform.position = new Vector3(pos.x + x, -y, pos.z + z);

                        combine.Add(new CombineInstance
                        {
                            mesh = blockMesh.sharedMesh,
                            transform = blockMesh.transform.localToWorldMatrix
                        });

                        if (Mathf.Abs(y) > 23)
                        {
                            float groundYScale = FindGroundYScale(y);

                            bool stone = false;
                            if (groundYScale >= 4) stone = true;

                            float groundY = blockMesh.transform.position.y + (blockMesh.transform.localScale.y / 2) + (groundYScale / 2);

                            blockMesh.transform.position = new Vector3(pos.x + x, groundY, pos.z + z);
                            blockMesh.transform.localScale = new Vector3(1, groundYScale, 1);

                            combine.Add(new CombineInstance
                            {
                                mesh = blockMesh.sharedMesh,
                                transform = blockMesh.transform.localToWorldMatrix
                            });

                            if (!stone)
                            {
                                spawnPoints.Add(blockMesh.transform);

                                int treeChance = Random.Range(0, 50);
                                if (treeChance == 0)
                                {
                                    GameObject curTree = GameObject.Instantiate(tree);
                                    curTree.transform.position = new Vector3(pos.x + x, groundY + .5f, pos.z + z);
                                    curTree.transform.parent = this.transform;
                                }
                            }
                            else
                            {
                                blockMesh.transform.position = new Vector3(pos.x + x, groundY + (groundYScale / 2) + 0.5f, pos.z + z);
                                blockMesh.transform.localScale = new Vector3(1, 1, 1);

                                combine.Add(new CombineInstance
                                {
                                    mesh = blockMesh.sharedMesh,
                                    transform = blockMesh.transform.localToWorldMatrix
                                });

                                if (groundYScale <= 3 || groundYScale == 10) spawnPoints.Add(blockMesh.transform);
                            }
                        }
                        else
                        {
                            float yIncrease = Random.Range(.5f, 4);
                            blockMesh.transform.localScale += new Vector3(0, yIncrease, 0);
                            blockMesh.transform.position += new Vector3(0, yIncrease / 2, 0);

                            combine.Add(new CombineInstance
                            {
                                mesh = blockMesh.sharedMesh,
                                transform = blockMesh.transform.localToWorldMatrix
                            });
                        }
                    }
                }
            }
        }

        CombineMesh(combine);

        IslandObjectSpawner islandObjectSpawner = this.GetComponent<IslandObjectSpawner>();
        islandObjectSpawner.IslandSpawned(spawnPoints);
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

    void CombineMesh(List<CombineInstance> combine)
    {
        // combine meshes
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine.ToArray());

        GameObject meshObject = new GameObject();
        meshObject.name = "meshObject";
        meshObject.transform.parent = this.transform;
        MeshFilter mf = meshObject.AddComponent<MeshFilter>();
        MeshRenderer mr = meshObject.AddComponent<MeshRenderer>();
        mf.mesh = combinedMesh;
        mr.material = material;
    }
}
