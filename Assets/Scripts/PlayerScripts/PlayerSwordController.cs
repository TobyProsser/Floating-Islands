using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    public float damage;

    public GameObject swordHolder;
    Animator swordAnimCon;

    int swingCount = -1;
    float swordTimer = 0;

    private void Awake()
    {
        swordAnimCon = swordHolder.GetComponent<Animator>();
        swordAnimCon.SetInteger("Swing", swingCount);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swordTimer = .5f;
        }

        if (swordTimer > -1) swordTimer -= Time.deltaTime;
        else if (swordTimer <= 0) swingCount = -1;

        swordAnimCon.SetFloat("SwingTime", swordTimer);
    }
}
