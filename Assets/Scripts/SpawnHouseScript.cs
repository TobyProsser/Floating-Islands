using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnHouseScript : MonoBehaviour
{
    public int maxHealth;
    int curHealth;

    public Slider healthSlider;

    void Start()
    {
        curHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    private void Update()
    {
        healthSlider.value = curHealth;

        if (curHealth <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            curHealth -= 10;
        }
    }
}
