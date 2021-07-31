using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    PlayerController playerController;

    public GameObject spawnShopCanvas;
    public FirstPersonLookController lookController;

    void Awake()
    {
        playerController = this.GetComponentInParent<PlayerController>();
        spawnShopCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SpawnShop") EnterShop();

        if (other.tag == "Coin")
        {
            playerController.coins++;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SpawnShop") ExitShop();
    }

    void EnterShop()
    {
        lookController.canLook = false;
        spawnShopCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitShop()
    {
        lookController.canLook = true;
        spawnShopCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
