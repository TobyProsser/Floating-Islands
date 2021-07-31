using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public float spawnRadius;
    public float spawnTime;

    public GameObject enemy;

    public List<Transform> spawnPoints = new List<Transform>();
    public List<Vector3> validPoints = new List<Vector3>();

    void Start()
    {
        FindSpawnPoints();
        StartCoroutine(spawnEnemies());
    }

    void FindSpawnPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Mathf.Abs(Vector3.Distance(spawnPoint.position, this.transform.position)) < spawnRadius)
            {
                validPoints.Add(spawnPoint.position);
            }
        }
    }

    IEnumerator spawnEnemies()
    {
        while (true)
        {
            Vector3 spawnPoint = validPoints[Random.Range(0, validPoints.Count)] + new Vector3(0, 1.5f, 0);
            GameObject curEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
