using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShopController : MonoBehaviour
{
    public float damageIncrease;

    public GameObject player;

    public void DamageIncrease(int num)
    {
        player.GetComponent<PlayerSwordController>().damage += damageIncrease * num;
    }

    public void SpreadIncrease()
    {
        player.GetComponent<SwordProjectileController>().spreadAmount++;
    }

    public void ProjectileAmount()
    {
        player.GetComponent<SwordProjectileController>().projectileAmount++;
    }
}
