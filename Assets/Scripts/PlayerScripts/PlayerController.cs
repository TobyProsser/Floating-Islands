using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHealth;
    float curHealth;

    public TextMeshProUGUI coinsText;
    public int coins;

    public GameObject house;

    SwordProjectileController projectileController;
    PlayerSwordController swordController;
    void Start()
    {
        
    }

    private void Update()
    {
        if (curHealth <= 0) OnDeath();
    }

    void LateUpdate()
    {
        coinsText.text = "Coins " + coins.ToString();
    }

    void OnDeath()
    {
        if (house == null)
        {
            //dont respawn
        }
        else
        {
            //respawn
            ResetSwordStats();
        }
    }

    void ResetSwordStats()
    {
        swordController.damage = 10;
        projectileController.projectileAmount = 1;
        projectileController.spreadAmount = 1;
    }
}
