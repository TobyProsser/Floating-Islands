using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    //accessed by EnemyCollisionDetection
    [HideInInspector]
    public float curHealth;

    public Slider healthSlider;
    //accessed by EnemyCollisionDetection
    [HideInInspector]
    public GameObject player;
    //accessed by EnemyCollisionDetection
    [HideInInspector]
    public PlayerSwordController swordController;

    public GameObject coin;

    void Start()
    {
        curHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    void Update()
    {
        healthSlider.value = curHealth;
        if (curHealth <= 0) OnDie();
    }

    void OnDie()
    {
        print(curHealth);
        SpawnCoins();
        Destroy(this.gameObject);
    }

    void SpawnCoins()
    {
        int coinsAmount = Random.Range(1, 5);
        for (int i = 0; i < coinsAmount; i++)
        {
            GameObject curCoin = Instantiate(coin, this.transform.position, Quaternion.identity);
            curCoin.GetComponent<Rigidbody>().AddExplosionForce(5, this.transform.position, 3);
        }
    }
}
