using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileController1 : MonoBehaviour
{
    public float speed = 10;
    public float damge;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Sword" && other.tag != "PlayerParent") Destroy(this.gameObject);
    }
}
