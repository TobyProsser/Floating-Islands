using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject player;

    public float timeBetweenMoves;
    public float walkDistance;

    bool agentStopped;

    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = null;
    }

    private void Start()
    {
        StartCoroutine(Walk());
    }

    void LateUpdate()
    {
        //if player isnt equal to null
        if (player != null)
        {
            //if the distance to player is greater than sum value
            if (Vector3.Distance(this.transform.position, player.transform.position) > 8)
            {
                //stop chasing after player
                StopCoroutine(MoveTowardsPlayerRoutine());
                player = null;
            }
            //if enemy is close enough to player, stop chasing player and stop agent
            else if (Vector3.Distance(this.transform.position, player.transform.position) < 6)
            {
                StopCoroutine(MoveTowardsPlayerRoutine());
                agent.isStopped = true;
                agentStopped = true;

                //If enemy is less than 3 units away from player, look at him
                this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            }
            else
            {
                //If enemy is less than 7 units away from player, look at him
                this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            }
        }

        //if enemy had been stopped
        if (agentStopped)
        {
            //check if the player is null, or if the player is far away from enemy and resume its wandering
            if (player == null || Vector3.Distance(this.transform.position, player.transform.position) > 4)
            {
                agentStopped = false;
                agent.isStopped = false;
                StartCoroutine(Walk());
            }
        }
    }

    IEnumerator Walk()
    {
        while (true)
        {
            try
            {
                //If the player isnt in range, wander
                if (player == null) agent.SetDestination(FindPoint());
            }
            catch (Exception)
            {
                print("could not set destination");
            }

            yield return new WaitForSeconds(timeBetweenMoves);
        }
    }

    IEnumerator MoveTowardsPlayerRoutine()
    {
        while (true)
        {
            try
            {
                agent.SetDestination(player.transform.position);
            }
            catch (Exception)
            {
                print("could not chase player");
            }
            yield return new WaitForSeconds(3);
        }
    }

    Vector3 FindPoint()
    {
        Vector3 walkToLoc;
        List<Transform> walkToBlocks = new List<Transform>();

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, walkDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Grass") walkToBlocks.Add(hitCollider.transform);
        }

        walkToLoc = walkToBlocks[UnityEngine.Random.Range(0, walkToBlocks.Count)].position;
        return walkToLoc;
    }

    bool IsPointOnNavMesh(Vector3 targetDestination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetDestination, out hit, 1f, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            StartCoroutine(MoveTowardsPlayerRoutine());
        }

        //health deducted by enemyHitDetection
    }
}
