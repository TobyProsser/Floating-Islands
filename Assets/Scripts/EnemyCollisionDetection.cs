using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDetection : MonoBehaviour
{
    EnemyController enemyController;

    private void Awake()
    {
        enemyController = this.GetComponentInParent<EnemyController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Sword")
        {
            enemyController.curHealth -= collision.transform.parent.GetComponentInParent<PlayerSwordController>().damage;
        }
    }
}
